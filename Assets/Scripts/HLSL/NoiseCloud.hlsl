float2 unity_gradientNoise_dir(float2 p)
{
    p = p % 289;
    float x = (34 * p.x + 1) * p.x % 289 + p.y;
    x = (34 * x + 1) * x % 289;
    x = frac(x / 41) * 2 - 1;
    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
}
 
float unity_gradientNoise(float2 p)
{
    float2 ip = floor(p);
    float2 fp = frac(p);
    float d00 = dot(unity_gradientNoise_dir(ip), fp);
    float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
    float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
    float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
}
 
void GradientCloudNoise_float(float2 UV, float Scale, out float Out)
{
    Out = unity_gradientNoise(UV * Scale) + 0.5;
}

float PerlinNoise(float2 UV, float Scale){
    return unity_gradientNoise(UV * Scale) + 0.5;
}


float checkSquare(float a, float b){
    float rn = 0;
    if( (a*a) <= (b*b)) rn = 1.0;
    return rn;
}


void SimplexNoise_float(float3 UV, float4 Scale, out float result)
{
    //result = frac( (sin(UV.x * Scale.x)*sin(UV.y * 17.233)*sin(UV.z * 22.73773) + cos(UV.y * 17.233)*cos(UV.x * 129.898)*cos(UV.z * 22.73773)) * 43758.5453 );
    float sdotp = dot(UV, Scale.xyz);
    //result = atan(tan(Scale.w * sin(sdotp)));
    //result = atan(tan(sdotp * Scale.w)) * sin(sdotp);
    result = 0.5f + pow(sin(Scale.w * sin(sdotp)), 3)/2;

    //result = frac(sin(UV.x * Scale.x + UV.y * Scale.y + UV.z * Scale.z) * Scale.w );
    //result = frac(sin(UV.x * Scale.x) * Scale.w) + frac(sin(UV.y * Scale.y) * Scale.w) + frac(sin(UV.z * Scale.z) * Scale.w);
    //result = frac( sin((UV.x  + UV.y  + UV.z)*Scale.x) * 43758.5453 );
}



float Simple3DNoise(float3 UV, float Scale)
{
    UV = UV * Scale;
    float3 posMin = floor(UV);
    float3 posMax = posMin + float3(1, 1, 1);

    //float2 min = (posMin/3.1415926535897932384626433);
    //float2 max = (posMax/3.1415926535897932384626433);

    //Out = lerp(min.x, max.x, UV.x - posMin.x) * lerp(min.y, max.y, UV.y - posMin.y);

    //Out = cos(UV.x * 3.14/Scale) * cos(UV.y* 3.14/Scale);
    //Out = frac(sin(dot(float2(UV.x, UV.y), float2(12.9898, 17.233))) * 43758.5453);

    float mixmiymiz = frac(sin(dot(float3(posMin.x, posMin.y, posMin.z), float3(12.9898, 17.233, 22.73773))) * 43758.5453);
    float mixmaymiz = frac(sin(dot(float3(posMin.x, posMax.y, posMin.z), float3(12.9898, 17.233, 22.73773))) * 43758.5453);
    float maxmiymiz = frac(sin(dot(float3(posMax.x, posMin.y, posMin.z), float3(12.9898, 17.233, 22.73773))) * 43758.5453);
    float maxmaymiz = frac(sin(dot(float3(posMax.x, posMax.y, posMin.z), float3(12.9898, 17.233, 22.73773))) * 43758.5453);

    float mixmiymaz = frac(sin(dot(float3(posMin.x, posMin.y, posMax.z), float3(12.9898, 17.233, 22.73773))) * 43758.5453);
    float mixmaymaz = frac(sin(dot(float3(posMin.x, posMax.y, posMax.z), float3(12.9898, 17.233, 22.73773))) * 43758.5453);
    float maxmiymaz = frac(sin(dot(float3(posMax.x, posMin.y, posMax.z), float3(12.9898, 17.233, 22.73773))) * 43758.5453);
    float maxmaymaz = frac(sin(dot(float3(posMax.x, posMax.y, posMax.z), float3(12.9898, 17.233, 22.73773))) * 43758.5453);

    float plane00 = lerp(mixmiymiz, mixmiymaz, UV.z - posMin.z);
    float plane01 = lerp(mixmaymiz, mixmaymaz, UV.z - posMin.z);
    float plane10 = lerp(maxmiymiz, maxmiymaz, UV.z - posMin.z);
    float plane11 = lerp(maxmaymiz, maxmaymaz, UV.z - posMin.z);

    float miyLerp = lerp(plane00, plane10, UV.x - posMin.x);
    float mayLerp = lerp(plane01, plane11, UV.x - posMin.x);

    return lerp(miyLerp, mayLerp, UV.y - posMin.y);
}


void NewNoise_float(float3 UV, float Scale, out float Out)
{
    Out = Simple3DNoise(UV, Scale);
}


//void Cloud_float( float3 rayOrigin, float3 rayDirection, float2 xzOffset, float scale, float numSteps, float stepSize,
//                     float densityScale, float boxLength, float3 boxPos, out float result )
//{
//	float density = 0;
	
//	for(int i = 0; i < numSteps; i++){
//		rayOrigin += (rayDirection*stepSize);
					
//		//Calculate density
//        float3 dist = rayOrigin - boxPos;
//        if(checkSquare(dist.x, boxLength) * checkSquare(dist.y, boxLength) * checkSquare(dist.z, boxLength) >= 1.0){
//		    density += PerlinNoise(rayOrigin.xy, scale) * PerlinNoise(rayOrigin.xz + xzOffset, scale) * PerlinNoise(rayOrigin.yz - xzOffset, scale);
//        }
//	}

//	result = density / densityScale;
//}

//float SampleDensity(float3 pos, float scale1, float){

//}

void Cloud_float( float3 rayOrigin, float3 rayDirection, float2 xzOffset, float scale, float numSteps, float stepSize,
                     float densityScale, float boxLength, float3 boxPos, out float result )
{
	float density = 0;
	
	for(int i = 0; i < numSteps; i++){
					
		//Calculate density
        float3 dist = rayOrigin - boxPos;
        if(checkSquare(dist.x, boxLength) * checkSquare(dist.y, boxLength) * checkSquare(dist.z, boxLength) >= 1.0){
		    density += PerlinNoise(rayOrigin.xy, scale) * PerlinNoise(rayOrigin.xz + xzOffset, scale) * PerlinNoise(rayOrigin.yz - xzOffset, scale) * 0.8f;
            //density += 0.1;
        }
		rayOrigin += (rayDirection*stepSize);
	}

	result = density / densityScale;
}


float3 TransLerp(float3 minOrg, float3 maxOrg, float3 valOrg, float3 minNew, float3 maxNew){
        return minNew + ((maxNew - minNew) * (valOrg - minOrg))/(maxOrg - minOrg);
}

float CubeIntersection_float(float3 cubePos, float3 cubeSize, float3 rayOrigin, float3 rayDirection, out float3 intersection){
    rayOrigin = TransLerp(cubePos - cubeSize/2, cubePos + cubeSize/2, rayOrigin, -cubeSize/2, cubeSize/2);

    //normalize(rayDirection);
    float3 absRayDir = abs(rayDirection);
    float3 antiAbs = rayDirection/absRayDir;

    float3 stepVec = ((antiAbs * cubeSize/2) - rayOrigin)/rayDirection;
    
    float stepMin = min(stepVec.x, stepVec.y);
    stepMin = min(stepMin,  stepVec.z);
    intersection = TransLerp(-cubeSize/2, cubeSize/2, rayOrigin + (rayDirection * stepMin), cubePos - cubeSize/2, cubePos + cubeSize/2);
    return stepMin;
}

float CubeIntersectionStepMin(float3 cubePos, float3 cubeSize, float3 rayOrigin, float3 rayDirection){
    rayOrigin = TransLerp(cubePos - cubeSize/2, cubePos + cubeSize/2, rayOrigin, -cubeSize/2, cubeSize/2);

    //normalize(rayDirection);
    float3 absRayDir = abs(rayDirection);
    float3 antiAbs = rayDirection/absRayDir;

    float3 stepVec = ((antiAbs * cubeSize/2) - rayOrigin)/rayDirection;
    
    float stepMin = min(stepVec.x, stepVec.y);
    stepMin = min(stepMin,  stepVec.z);
    return stepMin;
}

float GetDensity(float4 octave){
    //return PerlinNoise(octave.rg, octave.a) * PerlinNoise(octave.gb, octave.a) * PerlinNoise(octave.br, octave.a);
    return Simple3DNoise(octave.rgb, octave.a);
}

float MixDensity(float4 octaveA, float4 octaveB, float4 octaveC, float3 pos)
{
    return (0.5 * GetDensity(float4(octaveA.rgb + pos, octaveA.a))) + (0.3 * GetDensity(float4(octaveB.rgb + pos, octaveB.a))) + (0.2 * GetDensity(float4(octaveC.rgb + pos, octaveC.a)));
}

int WithinBox(float3 pos, float3 boxPos, float3 boxSize)
{
    float3 dist = pos - boxPos;
    return checkSquare(dist.x, boxSize.x/2) * checkSquare(dist.y, boxSize.y/2) * checkSquare(dist.z, boxSize.z/2);
}

void CloudOctaves_float( float3 rayOrigin, float3 rayDirection, float4 octaveA, float4 octaveB, float4 octaveC, float numSteps,
                     float densityScale, float3 boxSize, float3 boxPos, out float result )
{
	float density = 0;
    float stepSize = CubeIntersectionStepMin(boxPos, boxSize, rayOrigin, rayDirection);
    rayDirection *= (stepSize/numSteps);
	
	for(int i = 0; i < numSteps; i++){
					
		//Calculate density
        //float3 dist = rayOrigin - boxPos;
        //if(checkSquare(dist.x, boxSize.x/2) * checkSquare(dist.y, boxSize.y/2) * checkSquare(dist.z, boxSize.z/2) >= 1.0){
		    //density += (0.5 * GetDensity(float4(octaveA.rgb + rayOrigin, octaveA.a))) + (0.3 * GetDensity(float4(octaveB.rgb + rayOrigin, octaveB.a))) + (0.2 * GetDensity(float4(octaveC.rgb + rayOrigin, octaveC.a)));
            density += MixDensity(octaveA, octaveB, octaveC, rayOrigin);
            //density += 1;
        //}
		rayOrigin += rayDirection;
	}

	result = density / densityScale;
}


void CloudOctaves2_float( float3 rayOrigin, float3 rayDirection, float4 octaveA, float4 octaveB, float4 octaveC, float numSteps, float secondarySteps, float secStepSize,
                     float densityScale, float3 boxSize, float3 boxPos, float3 lightDirection, out float result, out float alpha )
{
	float density = 0;
    float stepSize = CubeIntersectionStepMin(boxPos, boxSize, rayOrigin, rayDirection);
    rayDirection *= (stepSize/numSteps);
	
	for(int i = 0; i < numSteps; i++)
    {
		density += (0.5 * GetDensity(float4(octaveA.rgb + rayOrigin, octaveA.a))) + (0.3 * GetDensity(float4(octaveB.rgb + rayOrigin, octaveB.a))) + (0.2 * GetDensity(float4(octaveC.rgb + rayOrigin, octaveC.a)));
        
        float secondaryStepSize = CubeIntersectionStepMin(boxPos, boxSize, rayOrigin, -lightDirection);
        lightDirection *= (secondaryStepSize/secondarySteps);
        float density2 = 0;
        for(int j = 0; j < secondarySteps; j++)
        {
            if(WithinBox(rayOrigin + (-lightDirection * j), boxPos, boxSize) >= 1){
                density2 += MixDensity(octaveA, octaveB, octaveC, rayOrigin + (-lightDirection * j *secStepSize));
            }
        }
        result += exp(-density2 * densityScale) * exp(-density * densityScale);
        alpha += density;
        rayOrigin += rayDirection;
	}
}


float HenyeyGreenstein(float3 vecA, float3 vecB, float g){
    float pi = 3.141592;
    float theta = dot(vecA, vecB) * pi;
    float num = (1 - pow(g, 2));
    float denum = 4 * pi * pow(1 + (g*g) - (2*g*cos(theta)), 3/2);
    return num/denum;
    //return dot(vecA, vecB);
}

void Cloud3_float( float3 rayOrigin, float3 rayDirection, float3 lightDirection, float densityPow, float densityScale, float4 octaveA, float4 octaveB, float4 octaveC, float numSteps, float secSteps, float transmittance, float scatteringFac, float3 boxSize, float3 boxPos, out float result, out float alpha)
{
	float density = 0;
    //float stepSize = CubeIntersectionStepMin(boxPos, boxSize, rayOrigin, rayDirection);
    //rayDirection *= (stepSize/numSteps);
	
	for(int i = 0; i < numSteps; i++)
    {
        if(WithinBox(rayOrigin, boxPos, boxSize) < 1) continue;

        float pointDensity = MixDensity(octaveA, octaveB, octaveC, rayOrigin);
        pointDensity = pow(pointDensity, densityPow);
        pointDensity *= densityScale;
        density += pointDensity;
		//density += MixDensity(octaveA, octaveB, octaveC, rayOrigin);
        
        //float secondaryStepSize = CubeIntersectionStepMin(boxPos, boxSize, rayOrigin, -lightDirection);
        //lightDirection *= (secondaryStepSize/secSteps);
        float density2 = 0;
        for(int j = 0; j < secSteps; j++)
        {
            if(WithinBox(rayOrigin + (-lightDirection * j), boxPos, boxSize) >= 1){
                density2 += MixDensity(octaveA, octaveB, octaveC, rayOrigin + (-lightDirection * j));
            }
        }
        result += HenyeyGreenstein(normalize(rayDirection), normalize(-lightDirection), scatteringFac) * exp(-density2 * transmittance) * exp(-density * transmittance);
        //alpha += density;
        rayOrigin += rayDirection;

        //density += MixDensity(octaveA, octaveB, octaveC, rayOrigin + (rayDirection * i));
        //result += secondaryStepSize;
	}
    //result = dot( normalize(rayDirection), normalize(lightDirection));

    //result =  exp(-density * transmittance);
    alpha = density;

    //alpha = 1;
    //if(result <= 1.0){
    //    result = 1.0;
    //}

    //result = 1;
    //alpha = 1;
}



void Cloud4_float( float3 rayOrigin, float3 rayDirection, float3 lightPos, float lightDirScale, float4 octaveA, float4 octaveB, float4 octaveC, float numSteps, float secSteps, float transmittance, float scatteringFac, float3 boxSize, float3 boxPos, out float result, out float alpha)
{
	float density = 0;
    float stepSize = CubeIntersectionStepMin(boxPos, boxSize, rayOrigin, rayDirection);
    rayDirection *= (stepSize/numSteps);
    float3 lightDir;
	
	for(int i = 0; i < numSteps; i++)
    {
        float pointDensity = MixDensity(octaveA, octaveB, octaveC, rayOrigin);
        density += pointDensity;
        lightDir = normalize(rayOrigin - lightPos) * lightDirScale;
		//density += MixDensity(octaveA, octaveB, octaveC, rayOrigin);
        
        //float secondaryStepSize = CubeIntersectionStepMin(boxPos, boxSize, rayOrigin, -lightPos);
        //lightPos *= (secondaryStepSize/secSteps);
        float density2 = 0;
        for(int j = 0; j < secSteps; j++)
        {
            if(WithinBox(rayOrigin + (-lightDir * j), boxPos, boxSize) >= 1){
                density2 += MixDensity(octaveA, octaveB, octaveC, rayOrigin + (-lightDir * j));
            }
        }
        result += HenyeyGreenstein(normalize(rayDirection), normalize(-lightDir), scatteringFac) * pointDensity * exp(-density2 * transmittance) * exp(-density * transmittance);
        //alpha += density;
        rayOrigin += rayDirection;

        //density += MixDensity(octaveA, octaveB, octaveC, rayOrigin + (rayDirection * i));
        //result += secondaryStepSize;
	}
    //result = dot( normalize(rayDirection), normalize(lightPos));

    //result =  exp(-density * transmittance);
    alpha = density;

    //alpha = 1;
    //if(result <= 1.0){
    //    result = 1.0;
    //}

    //result = 1;
    //alpha = 1;
}







