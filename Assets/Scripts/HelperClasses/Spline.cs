using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline
{
    public Vector3 Start 
    { 
        get { return new Vector3(curvex.y1, 0, curvey.y1); } 
        set { curvex.y1 = value.x; curvey.y1 = value.z; } 
    }

    public Vector3 End
    {
        get { return new Vector3(curvex.y2, 0, curvey.y2); }
        set { curvex.y2 = value.x; curvey.y2 = value.z;}
    }

    public Vector3 StartDir
    {
        get { return new Vector3(curvex.d1, 0, curvey.d1); }
        set { curvex.d1 = value.x; curvey.d1 = value.z; }
    }

    public Vector3 EndDir
    {
        get { return new Vector3(curvex.d2, 0, curvey.d2); }
        set { curvex.d2 = value.x; curvey.d2 = value.z; }
    }

    private C1CubicCurve curvex;
    private C1CubicCurve curvey;

    public Spline()
    {
        curvex = new C1CubicCurve();
        curvey = new C1CubicCurve();
    }

    public Spline(Vector3 start, Vector3 startDir, Vector3 end, Vector3 endDir)
    {
        curvex = new C1CubicCurve(start.x, startDir.x, end.x, endDir.x);
        curvey = new C1CubicCurve(start.z, startDir.z, end.z, endDir.z);
    }

    public void Update()
    {
        curvex.CalculateCoefficients();
        curvey.CalculateCoefficients();
    }
    public void Update(Vector3 start, Vector3 startDir, Vector3 end, Vector3 endDir)
    {
        curvex.CalculateCoefficients(start.x, startDir.x, end.x, endDir.x);
        curvey.CalculateCoefficients(start.z, startDir.z, end.z, endDir.z);
    }

    public Vector3 GetPosAt(float t)
    {
        return new Vector3(curvex.GetValAt(t), 0, curvey.GetValAt(t));
    }
}


public class C1CubicCurve
{
    private float a, b, c, d;
    public float A => a;
    public float B => b;
    public float C => c;
    public float D => d;

    public float y1, d1, t1, y2, d2, t2;
    public void CalculateCoefficients()
    {

        float t1square = t1 * t1;
        float t2square = t2 * t2;
        float t1cube = (t1 * t1 * t1);
        float t2cube = (t2 * t2 * t2);
        float f = (t1cube - t2cube) - (3 * t1square * (t1 - t2));
        float g = (t1square - t2square) - (2 * t1 * (t1 - t2));
        float h = 2 * (t1 - t2);
        float i = 3 * (t1square - t2square);

        a = (((y1 - y2 - (d1 * (t1 - t2))) * h) - (g * (d1 - d2))) / ((f * h) - (g * i));
        b = (d1 - d2 - (a * i)) / h;
        c = d1 - (3 * a * t1square) - (2 * b * t1);
        d = y1 - (a * t1cube) - (b * t1square) - (c * t1);
    }

    //public void CalculateCoefficients(float Y1, float D1, float Y2, float D2)
    //{
    //    y1 = Y1;
    //    y2 = Y2;
    //    d1 = D1;
    //    d2 = D2;

    //    float z = (3 * y2) - (d2 * t2);
    //    float y = z - ((3 * y1) - (d1 * t1));
    //    float x = (y * (t2 + t1)) - (2 * d1 * t2 * t2) + (2 * d2 * t1 * t1);
    //    float w = ((t2 * t2) - (t1 * t1));
    //    float v = (w * (t2 + t1)) - (4 * t1 * t2 * (t2 - t1));
    //    b = x / v;
    //    //c = ((-y) - (b * (-w))) / (2 * (t1 - t2));
    //    c = (((3 * y1) - (d1 * t1)) - ((3 * y2) - (d2 * t2)) - (b * (-w))) / (2 * (t1 - t2));
    //    d = ((3 * y1) - (d1 * t1) - (b * t1 * t1) - (2 * c * t1))/3;
    //    a = (t1 == 0) ? 0 : (y1 - (b * t1 * t1) - (c * t1) - d) / (t1 * t1 * t1);

    //    //Debug.Log("z: " + z + " y: " + y + " x: " + x + " w: " + w + " v: " + v);
    //    //Debug.Log("b: " + b + " c: " + c + " d: " + d + " a: " + a);
    //}

    public void CalculateCoefficients(float Y1, float D1, float Y2, float D2)
    {
        y1 = Y1;
        y2 = Y2;
        d1 = D1;
        d2 = D2;

        //float t1square = t1 * t1;
        //float t2square = t2 * t2;
        //float t1cube = (t1 * t1 * t1);
        //float t2cube = (t2 * t2 * t2);
        //float f = (t1cube - t2cube) - (3 * t1square * (t1 - t2));
        //float g = (t1square - t2square) - (2 * t1 * (t1 - t2));
        //float h = 2 * (t1 - t2);
        //float i = 3 * (t1square - t2square);

        //a = (((y1 - y2 - (d1 * (t1 - t2))) * h) - (g * (d1 - d2))) / ((f * h) - (g * i));
        //b = (d1 - d2 - (a * i)) / h;
        //c = d1 - (3 * a * t1square) - (2 * b * t1);
        //d = y1 - (a * t1cube) - (b * t1square) - (c * t1);

        CalculateCoefficients();
    }

    public C1CubicCurve()
    {
        t1 = 0;
        t2 = 1;
    }
    public C1CubicCurve(float Y1, float D1, float Y2, float D2)
    {
        y1 = Y1;
        y2 = Y2;
        d1 = D1;
        d2 = D2;
        t1 = 0;
        t2 = 1;

        CalculateCoefficients();
    }

    public C1CubicCurve(float Y1, float D1, float Y2, float D2, float T1, float T2)
    {
        y1 = Y1;
        y2 = Y2;
        d1 = D1;
        d2 = D2;
        t1 = T1;
        t2 = T2;

        CalculateCoefficients();
    }

    public float GetValAt(float t)
    {
        return (a * t * t * t) + (b * t * t) + (c * t) + d;
    }

}
