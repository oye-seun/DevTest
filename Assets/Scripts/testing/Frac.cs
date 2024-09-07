using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frac : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(frac(5.4f));
        Debug.Log(frac(7.2f));
        Debug.Log(frac(8.7999f));
        Debug.Log(frac(9));

        //Debug.Log(Math.Sin(120 * Math.PI));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private double frac(float no)
    {
        return 0.5f + Math.Atan(Math.Sin(2 * Math.PI * no) / (Math.Cos(2 * Math.PI * no) - 1)) / Math.PI;
    }
}
