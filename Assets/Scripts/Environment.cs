using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public Vector3 pos1;
    public Vector3 pos2;
    public float timescale;
    public float timeshift;

    
    // Update is called once per frame
    void Update()
    {
        transform.position = interpolatevector(pos1, pos2, Mathf.Cos((Time.time * timescale) + timeshift), -1, 1);
    }

    public static Vector3 interpolatevector(Vector3 pos1, Vector3 pos2, float val, float min, float max)
    {
        //Debug.Log(val);
        Vector3 midpoint = new Vector3(
            interpolate(min, max, val, pos1.x, pos2.x),
            interpolate(min, max, val, pos1.y, pos2.y),
            interpolate(min, max, val, pos1.z, pos2.z));

        return midpoint;
    }

    public static float interpolate(float min1, float max1, float val1, float min2, float max2)
    {
        float val2 = min2 + ((val1 - min1) * ( max2 - min2) / (max1 - min1));
        return val2;
    }
}
