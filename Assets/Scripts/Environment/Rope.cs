using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    //[SerializeField] private int _stepCount;
    ////[SerializeField] private float _massPerFloat;
    //[SerializeField] private float _stepDist;
    //[SerializeField] private Transform _start;
    //[SerializeField] private Transform _end;

    ////private float C;
    ////private float G;
    ////private float H;
    ////private float I;
    ////private float sumDSquare;

    //private float a, b, c, n, t, e, f, g, ay, ax;
    //private float sumDSquare;

    //private LineRenderer _lineRenderer;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _lineRenderer = GetComponent<LineRenderer>();
    //    _lineRenderer.positionCount = _stepCount;

    //    //C = -Physics.gravity.y * Time.fixedDeltaTime;
    //    //G = 4 * _stepCount * C * C;
    //    //H = 4 * _stepCount * _stepCount * C * C * C;
    //    //I = C * C * C * C * (((6 * _stepCount) + ((8 * _stepCount) * ((_stepCount * _stepCount) - 1))) / 6);
    //    //sumDSquare = (_stepDist * _stepDist) * _stepCount;
    //    //Debug.Log("C: " + C + " G: " + G + " H: " + H + " I: " + I);

    //    //t = Time.fixedDeltaTime;
    //    //n = 10;
    //    //float Uy = 25;
    //    //float cy = -Physics.gravity.y * t;
    //    //float Gy = 4 * n * cy * cy;
    //    //float Hy = cy * cy * cy * 4 * n * n;
    //    //float iy = cy * cy * cy * cy * ((6 * n) + ((8 * n) * ((n * n) - 1))) / 6;
    //    //float sumSysquare = ((Gy * Uy * Uy) + (Hy * Uy) + iy)/(4 * -Physics.gravity.y * -Physics.gravity.y);

    //    //Debug.Log("Formula val: " + sumSysquare);

    //    //float SSS = 0;
    //    //for (int i = 0; i < 10; i++)
    //    //{
    //    //    SSS += Mathf.Pow (Uy * t, 2);
    //    //    Uy += cy;
    //    //}

    //    //Debug.Log("Function val: " + SSS);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //float xDisp = _end.position.x - _start.position.x;
    //    ////float xVel = ((2*xDisp) - (-C * Time.fixedDeltaTime))/(2 * Time.fixedDeltaTime);
    //    //float xVel = xDisp / (_stepCount * Time.fixedDeltaTime);
    //    //float det = (H * H) - (4 * G * ((2 * I) + (G * xVel * xVel) + (H * xVel) - sumDSquare));
    //    ////Debug.Log("xVel: " + xVel + " det: " + det);


    //    n = _stepCount;
    //    t = Time.fixedDeltaTime;
    //    ay = -Physics.gravity.y;
    //    ax = 0.0001f;
    //    sumDSquare = (_stepDist * _stepDist) * n;

    //    float cy = ay * t;
    //    float Gy = (4 * n * cy * cy) / (4 * ay * ay);
    //    float Hy = -(cy * cy * cy * 4 * n * n) / (4 * ay * ay);
    //    float iy = (cy * cy * cy * cy * ((6 * n) + ((8 * n) * ((n * n) - 1))) / 6) / (4 * ay * ay);
    //    float cx = ax * t;
    //    float Gx = (4 * n * cx * cx) / (4 * ax * ax);
    //    float Hx = -(cx * cx * cx * 4 * n * n) / (4 * ax * ax);
    //    float ix = (cx * cx * cx * cx * ((6 * n) + ((8 * n) * ((n * n) - 1))) / 6) / (4 * ax * ax);

    //    a = (n * t * t) /*- Gy*/;
    //    b = (ay * t * t * t * (n * (n - 1))) /*- Hy*/;
    //    f = ((ax * ax * t * t * t * t) + (ay * ay * t * t * t * t)) * ((n * (n - 1) * ((2 * n) - 1)) / 6);
    //    float xDisp = _end.position.x - _start.position.x;
    //    float ux = xDisp / (_stepCount * Time.fixedDeltaTime);
    //    e = ux * ((ax * t * t * t * (n * (n - 1))) /*- Hx*/);
    //    g = ux * ux * ((n * t * t) /*- Gx*/);
    //    c = -sumDSquare + e + f + g /*- iy - ix*/;

    //    //a = Gy;
    //    //b = Hy;
    //    //c = iy + (Gx * ux * ux) + (Hx * ux) + ix - sumDSquare;
    //    float det = (b * b) - (4 * a * c);
    //    Debug.Log("xVel: " + ux + " det: " + det);

    //    //float disp =  _start.position.y - _end.position.y;
    //    //float vuy = Mathf.Sqrt(Mathf.Abs(2 * ay * disp));
    //    //vuy *= Mathf.Sign(ay * disp);
    //    //float uy = (vuy) - (ay * n * t);
    //    //det = 1;

    //    if (det < 0)
    //    {
    //        ////Debug.Log("complex root");
    //        ////return;
    //        ////float yVel = (-H + Mathf.Sqrt(-det) < 0) ? (-H + Mathf.Sqrt(-det)) / (2 * G) : (-H - Mathf.Sqrt(-det)) / (2 * G);
    //        //float yVel = (-b + Mathf.Sqrt(-det) < 0) ? (-b + Mathf.Sqrt(-det)) / (2 * a) : (-b - Mathf.Sqrt(-det)) / (2 * a);
    //        //Debug.Log("yVel: " + yVel);
    //        //Vector3 vel = new Vector3(ux, yVel, 0);
    //        //Vector3 str = _start.position;
    //        //for (int i = 0; i < _stepCount; i++)
    //        //{
    //        //    Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //        //    Debug.DrawLine(str, newStr);
    //        //    vel += -Physics.gravity * Time.fixedDeltaTime;
    //        //    str = newStr;
    //        //}
    //    }
    //    else
    //    {
    //        //float yVel = (-H + Mathf.Sqrt(det) < 0)? (-H + Mathf.Sqrt(det)) / (2 * G) : (-H - Mathf.Sqrt(det)) / (2 * G);
    //        ////Debug.Log("yVel: " + yVel);


    //        float yVel = (-b + Mathf.Sqrt(det) < 0) ? (-b + Mathf.Sqrt(det)) / (2 * a) : (-b - Mathf.Sqrt(det)) / (2 * a);
    //        //float yVel = (-b + Mathf.Sqrt(det)) / (2 * a);
    //        Debug.Log("yVel: " + yVel);
    //        //float yVel = uy;

    //        Vector3 vel = new Vector3(ux, yVel, 0);
    //        Vector3 str = _start.position;
    //        for (int i = 0; i < _stepCount; i++)
    //        {
    //            Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //            Debug.DrawLine(str, newStr);
    //            vel += -Physics.gravity * Time.fixedDeltaTime;
    //            str = newStr;
    //        }
    //    }
    //}




    //[SerializeField] private int _stepCount;
    ////[SerializeField] private float _massPerFloat;
    //[SerializeField] private float _stepDist;
    //[SerializeField] private Transform _start;
    //[SerializeField] private Transform _end;

    ////private float C;
    ////private float G;
    ////private float H;
    ////private float I;
    ////private float sumDSquare;

    //private float a, b, c, n, t, e, f, g, ay, ax;
    //private float sumDSquare;

    //private LineRenderer _lineRenderer;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _lineRenderer = GetComponent<LineRenderer>();
    //    _lineRenderer.positionCount = _stepCount;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //float xDisp = _end.position.x - _start.position.x;
    //    ////float xVel = ((2*xDisp) - (-C * Time.fixedDeltaTime))/(2 * Time.fixedDeltaTime);
    //    //float xVel = xDisp / (_stepCount * Time.fixedDeltaTime);
    //    //float det = (H * H) - (4 * G * ((2 * I) + (G * xVel * xVel) + (H * xVel) - sumDSquare));
    //    ////Debug.Log("xVel: " + xVel + " det: " + det);


    //    n = _stepCount;
    //    t = Time.fixedDeltaTime;
    //    ay = -Physics.gravity.y;
    //    ax = 0;
    //    //sumDSquare = (_stepDist * _stepDist) * n;
    //    sumDSquare = Mathf.Pow(_stepDist * n, 2);
    //    //a = n * t * t;
    //    //b = ax * t * t * t * (n * (n - 1));
    //    //f = ((ax * ax * t * t * t * t) + (ay * ay * t * t * t * t)) * ((n * (n - 1) * ((2 * n) - 1)) / 6);
    //    float uy = ((2 * ay * (_end.position.y - _start.position.y)) - (ay * ay * n * n * t * t)) / (2 * ay * n * t);

    //    a = n * n * t * t;
    //    b = ax * n * n * t * t * t * (n - 1);
    //    f = ((/*(ay * ay) +*/ (ax * ax)) * n * n * t * t * t * t * (n - 1) * (n - 1)) / 4;

    //    e = Mathf.Pow(_end.position.y - _start.position.y, 2);
    //    c = -sumDSquare + e + f /*+ g*/ /*- iy - ix*/;


    //    //e = uy * ay * t * t * t * (n * (n - 1));
    //    //g = n * uy * uy * t * t;
    //    //c = -sumDSquare + e + f + g;
    //    float det = (b * b) - (4 * a * c);
    //    //Debug.Log("xVel: " + xVel + " det: " + det);




    //    //det = 1;

    //    if (det < 0)
    //    {
    //        Debug.Log("complex root");
    //        return;
    //        //float yVel = (-H + Mathf.Sqrt(-det) < 0) ? (-H + Mathf.Sqrt(-det)) / (2 * G) : (-H - Mathf.Sqrt(-det)) / (2 * G);
    //        ////Debug.Log("yVel: " + yVel);
    //        //Vector3 vel = new Vector3(xVel, yVel, 0);
    //        //Vector3 str = _start.position;
    //        //for (int i = 0; i < _stepCount; i++)
    //        //{
    //        //    Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //        //    Debug.DrawLine(str, newStr);
    //        //    vel += -Physics.gravity * Time.fixedDeltaTime;
    //        //    str = newStr;
    //        //}
    //    }
    //    else
    //    {
    //        //float yVel = (-H + Mathf.Sqrt(det) < 0)? (-H + Mathf.Sqrt(det)) / (2 * G) : (-H - Mathf.Sqrt(det)) / (2 * G);
    //        ////Debug.Log("yVel: " + yVel);


    //        //float yVel = (-b + Mathf.Sqrt(det) < 0) ? (-b + Mathf.Sqrt(det)) / (2 * a) : (-b - Mathf.Sqrt(det)) / (2 * a);
    //        float ux = (-b + Mathf.Sqrt(det)) / (2 * a);
    //        //float ux = Mathf.Sqrt(Mathf.Abs(c/a));
    //        //float yVel = uy;

    //        Vector3 vel = new Vector3(ux, uy, 0);
    //        Vector3 str = _start.position;
    //        for (int i = 0; i < _stepCount; i++)
    //        {
    //            Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //            Debug.DrawLine(str, newStr);
    //            vel += -Physics.gravity * Time.fixedDeltaTime;
    //            str = newStr;
    //        }
    //    }
    //}






    //[SerializeField] private int _stepCount;
    ////[SerializeField] private float _massPerFloat;
    //[SerializeField] private float _stepDist;
    //[SerializeField] private float _ropeLength;
    //[SerializeField] private Transform _start;
    //[SerializeField] private Transform _end;

    //private float a, b, c, n, t, e, f, g, ay, ax;
    //private float sumDSquare;

    //private LineRenderer _lineRenderer;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _lineRenderer = GetComponent<LineRenderer>();
    //    _lineRenderer.positionCount = _stepCount;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    n = _stepCount;
    //    t = Time.fixedDeltaTime;
    //    ay = -Physics.gravity.y;
    //    ax = 0;
    //    sumDSquare = Mathf.Pow(_stepDist, 2) * n;

    //    //float len = Mathf.Pow(_start.position.y - _end.position.y, 2) + Mathf.Pow(_start.position.x - _end.position.x, 2);
    //    //sumDSquare = /*(len < _ropeLength * _ropeLength) ? len : */_ropeLength * _ropeLength;

    //    //float uy = ((2 * ay * (_end.position.y - _start.position.y)) - (ay * ay * n * n * t * t)) / (2 * ay * n * t);

    //    a = n * t * t;
    //    b = ay * n * t * t * t * (n - 1);
    //    f = t * t * t * t * ((ay * ay) + (ax * ax)) * ((n*n*n/3) - (n*n/2) + (n/4));

    //    float xDisp = _end.position.x - _start.position.x;
    //    float ux = xDisp / (_stepCount * Time.fixedDeltaTime);
    //    g = ux * ux * a;
    //    e = ux * ax * n * t * t * t * (n - 1);
    //    //e = Mathf.Pow(xDisp, 2);
    //    c = -sumDSquare + e + f + g /*- iy - ix*/;

    //    float det = (b * b) - (4 * a * c);
    //    //Debug.Log("xVel: " + ux + " det: " + det);
    //    //Debug.Log("sum D square: " + sumDSquare + " xdisp: " + xDisp  + " det: " + det);


    //    if (det < 0)
    //    {
    //        ////Debug.Log("complex root");
    //        ////return;
    //        ////float yVel = (-H + Mathf.Sqrt(-det) < 0) ? (-H + Mathf.Sqrt(-det)) / (2 * G) : (-H - Mathf.Sqrt(-det)) / (2 * G);
    //        //float yVel = (-b + Mathf.Sqrt(-det) < 0) ? (-b + Mathf.Sqrt(-det)) / (2 * a) : (-b - Mathf.Sqrt(-det)) / (2 * a);
    //        //Debug.Log("yVel: " + yVel);
    //        //Vector3 vel = new Vector3(ux, yVel, 0);
    //        //Vector3 str = _start.position;
    //        //for (int i = 0; i < _stepCount; i++)
    //        //{
    //        //    Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //        //    Debug.DrawLine(str, newStr);
    //        //    vel += -Physics.gravity * Time.fixedDeltaTime;
    //        //    str = newStr;
    //        //}
    //    }
    //    else
    //    {
    //        //float yVel = (-H + Mathf.Sqrt(det) < 0)? (-H + Mathf.Sqrt(det)) / (2 * G) : (-H - Mathf.Sqrt(det)) / (2 * G);
    //        ////Debug.Log("yVel: " + yVel);


    //        //float yVel = (-b + Mathf.Sqrt(det) < 0) ? (-b + Mathf.Sqrt(det)) / (2 * a) : (-b - Mathf.Sqrt(det)) / (2 * a);
    //        //float yVel = (sumDSquare - a - f - e) / b;

    //        //a = n * n * t * t;
    //        //b = ax * n * n * t * t * t * (n - 1);
    //        //f = ((/*(ay * ay) +*/ (ax * ax)) * n * n * t * t * t * t * (n - 1) * (n - 1)) / 4;

    //        //e = Mathf.Pow(_end.position.y - _start.position.y, 2);
    //        //c = -sumDSquare + e + f /*+ g*/ /*- iy - ix*/;

    //        //float det2 = (b * b) - (4 * a * c);
    //        //ux = (det2 >= 0) ? (-b + Mathf.Sqrt(det2)) / (2 * a) : ux;

    //        float yVel = (-b + Mathf.Sqrt(det)) / (2 * a);
    //        Debug.Log("yVel: " + yVel);
    //        //float yVel = uy;

    //        Vector3 vel = new Vector3(ux, yVel, 0);
    //        Vector3 str = _start.position;
    //        for (int i = 0; i < _stepCount; i++)
    //        {
    //            Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //            Debug.DrawLine(str, newStr);
    //            vel += -Physics.gravity * Time.fixedDeltaTime;
    //            str = newStr;
    //        }
    //    }
    //}






    //[SerializeField] private int _stepCount;
    //[SerializeField] private float _stepDist;
    //[SerializeField] private float _timeStep;
    ////[SerializeField] private float _totalVel;
    //[SerializeField] [Range(0f, 1f)] private float _ratio;
    //[SerializeField] private Transform _start;
    //[SerializeField] private Transform _end;


    //private float A, B, C;
    //private float a, b, c, n, t, e, f, g, ay, ax;
    //[SerializeField] private float _sumDSquare;

    //private LineRenderer _lineRenderer;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _lineRenderer = GetComponent<LineRenderer>();
    //    _lineRenderer.positionCount = _stepCount;
    //}

    //private float CubeRoot(float num)
    //{
    //    float sign = Mathf.Sign(num);
    //    return sign * Mathf.Pow(Mathf.Abs(num), 1f / 3);
    //}
    //// Update is called once per frame
    //void Update()
    //{
    //    n = _stepCount;
    //    t = Time.fixedDeltaTime /*0.1f*/;
    //    ay = -Physics.gravity.y;
    //    ax = 0;
    //    //sumDSquare = (_stepDist * _stepDist) * n;

    //    float xDisp = _end.position.x - _start.position.x;
    //    float ux = xDisp / (_stepCount * t);
    //    float uy = ((2 * ay * (_end.position.y - _start.position.y)) - (ay * ay * n * n * t * t)) / (2 * ay * n * t) * _ratio;
    //    //float ux = Mathf.Sqrt((_totalVel * _totalVel) - (uy * uy));
    //    //float uy = (_end.position.y - _start.position.y)/ (_stepCount * Time.fixedDeltaTime);


    //    A = t * t * t * t * ((ax * ax) + (ay * ay));
    //    B = t * t * t * ((uy * ay) + (ux * ax));
    //    C = t * t * ((ux * ux) + (uy * uy));

    //    a = (2 * A) / 6;
    //    b = B - (3 * A / 6);
    //    c = C - B + (A / 6);
    //    float seriesSDSquare = (n * n * n * a) + (n * n * b) + (n * c);


    //    A = (ax * ax) + (ay * ay);
    //    B = (uy * ay) + (ux + ax);
    //    C = (ux * ux) + (uy * uy);


    //    a = A / 3;
    //    b = B;
    //    c = C;
    //    float integralSDSquare = (n * n * n * a) + (n * n * b) + (n * c);

    //    float yVel = uy;

    //    Vector3 vel = new Vector3(ux, yVel, 0);
    //    Vector3 str = _start.position;
    //    float sqrDist = 0;
    //    for (int i = 0; i < _stepCount; i++)
    //    {
    //        Vector3 newStr = str + (vel * t /*stepTime*/);
    //        sqrDist += Vector3.SqrMagnitude(newStr - str);
    //        Debug.DrawLine(str, newStr);
    //        vel += new Vector3(ax, ay, 0) * t /*stepTime*/;
    //        str = newStr;
    //    }
    //    Debug.Log("integralSDSquare: " + integralSDSquare + " seriesSDSquare: " + seriesSDSquare + " sqrDist: " + sqrDist);
    //}






    //[SerializeField] private int _stepCount;
    //[SerializeField] private Transform _start;
    //[SerializeField] private Transform _end;
    //[SerializeField] private float _sumDSquare;

    //private float a, b, c, n, t, e, f, g, ay, ax;

    //private LineRenderer _lineRenderer;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _lineRenderer = GetComponent<LineRenderer>();
    //    _lineRenderer.positionCount = _stepCount;

    //    //Debug.Log("epsilon: " + 2.7182818);
    //    //Debug.Log("log(2, e): " + Mathf.Log(2, 2.7182818f));
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    n = _stepCount;
    //    t = Time.fixedDeltaTime;
    //    ay = -Physics.gravity.y;
    //    ax = 0;

    //    float xDisp = _end.position.x - _start.position.x;
    //    float ux = xDisp / (_stepCount * Time.fixedDeltaTime);
    //    float uy = ((2 * ay * (_end.position.y - _start.position.y)) - (ay * ay * n * n * t * t)) / (2 * ay * n * t);

    //    a = t * t * t * t * (n * (n - 1) * ((2 * n) - 1) / 6);
    //    b = uy * t * t * t * n * (n-1);

    //    e =  n * t * t * ((ux * ux) + (uy * uy));
    //    f = ux * ax * t * t * t * n * (n - 1);
    //    g = a * ax * ax;

    //    c = e + f + g - _sumDSquare;

    //    float det = (b * b) - (4 * a * c);
    //    Debug.Log("det: " + det);
    //    //Debug.Log("sum D square: " + sumDSquare + " xdisp: " + xDisp  + " det: " + det);


    //    if (det < 0)
    //    {
    //        ////Debug.Log("complex root");
    //        ////return;
    //        ////float yVel = (-H + Mathf.Sqrt(-det) < 0) ? (-H + Mathf.Sqrt(-det)) / (2 * G) : (-H - Mathf.Sqrt(-det)) / (2 * G);
    //        //float yVel = (-b + Mathf.Sqrt(-det) < 0) ? (-b + Mathf.Sqrt(-det)) / (2 * a) : (-b - Mathf.Sqrt(-det)) / (2 * a);
    //        //Debug.Log("yVel: " + yVel);
    //        //Vector3 vel = new Vector3(ux, yVel, 0);
    //        //Vector3 str = _start.position;
    //        //for (int i = 0; i < _stepCount; i++)
    //        //{
    //        //    Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //        //    Debug.DrawLine(str, newStr);
    //        //    vel += -Physics.gravity * Time.fixedDeltaTime;
    //        //    str = newStr;
    //        //}
    //    }
    //    else
    //    {
    //        float newAy = (-b + Mathf.Sqrt(det) == 0) ? 0 : (-b + Mathf.Sqrt(det)) / (2 * a);
    //        Debug.Log("ay: " + newAy);
    //        //float yVel = uy;

    //        Vector3 vel = new Vector3(ux, uy, 0);
    //        Vector3 str = _start.position;
    //        for (int i = 0; i < _stepCount; i++)
    //        {
    //            Vector3 newStr = str + (vel * t);
    //            Debug.DrawLine(str, newStr);
    //            vel += new Vector3(ax, newAy, 0) * t;
    //            str = newStr;
    //        }
    //    }
    //}







    [SerializeField] private int _stepCount;
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    [SerializeField] private float _sumDSquare;
    [SerializeField] private float _ax, _az;

    private float a, b, c, n, t, e, f, g, ay, ax, az;

    private LineRenderer _lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _stepCount;

        //Debug.Log("epsilon: " + 2.7182818);
        //Debug.Log("log(2, e): " + Mathf.Log(2, 2.7182818f));
    }

    // Update is called once per frame
    void Update()
    {
        n = _stepCount;
        t = Time.fixedDeltaTime;
        //ay = -Physics.gravity.y;
        ax = _ax;
        az = _az;


        Vector3 startPos = Vector3.zero;
        Vector3 endPos = Vector3.zero;

        startPos = _start.position;
        endPos = _end.position;

        //if (_start.position.y >= _end.position.y)
        //{
        //    startPos = _start.position;
        //    endPos = _end.position;
        //}
        //else
        //{
        //    endPos = _start.position;
        //    startPos = _end.position;
        //}


        float xDisp = endPos.x - startPos.x;
        float yDisp = endPos.y - startPos.y;
        float zDisp = endPos.z - startPos.z;
        //float ux = xDisp / (n * t);
        float ux = (xDisp - (0.5f * ax * n * n * t * t)) / (n * t);
        float uz = (zDisp - (0.5f * az * n * n * t * t)) / (n * t);
        //float uy = ((2 * ay * (_end.position.y - _start.position.y)) - (ay * ay * n * n * t * t)) / (2 * ay * n * t);

        float A = t * t * t * t * (n * (n - 1) * ((2 * n) - 1) / 6);
        float B = t * t * (n - 1);

        a = A - (B * t * t * n * n/2) + (n * n * n * t * t * t * t/4);
        b = (B * yDisp) - (n * t * t * yDisp);

        e = n * t * t * ((ux * ux) + (uz * uz));
        f = ((ux * ax) + (uz * az)) * t * t * t * n * (n - 1);

        c = (yDisp * yDisp/n) - _sumDSquare + (((ax * ax) + (az * az)) * A) + f + e;

        float det = (b * b) - (4 * a * c);
        Debug.Log("det: " + det);
        //Debug.Log("sum D square: " + sumDSquare + " xdisp: " + xDisp  + " det: " + det);


        if (det < 0)
        {
            ////Debug.Log("complex root");
            ////return;
            ////float yVel = (-H + Mathf.Sqrt(-det) < 0) ? (-H + Mathf.Sqrt(-det)) / (2 * G) : (-H - Mathf.Sqrt(-det)) / (2 * G);
            //float yVel = (-b + Mathf.Sqrt(-det) < 0) ? (-b + Mathf.Sqrt(-det)) / (2 * a) : (-b - Mathf.Sqrt(-det)) / (2 * a);
            //Debug.Log("yVel: " + yVel);
            //Vector3 vel = new Vector3(ux, yVel, 0);
            //Vector3 str = _start.position;
            //for (int i = 0; i < _stepCount; i++)
            //{
            //    Vector3 newStr = str + (vel * Time.fixedDeltaTime);
            //    Debug.DrawLine(str, newStr);
            //    vel += -Physics.gravity * Time.fixedDeltaTime;
            //    str = newStr;
            //}
        }
        else
        {
            float newAy = (-b + Mathf.Sqrt(det) == 0) ? 0 : (-b + Mathf.Sqrt(det)) / (2 * a);
            
            //if(newAy < 0) newAy = (-b - Mathf.Sqrt(det)) / (2 * a);
            if (newAy >= 0)
            {

                float uy = (yDisp - (0.5f * newAy * n * n * t * t)) / ( n * t);
                //float yVel = uy;

                Vector3 vel = new Vector3(ux, uy, uz);
                Vector3 str = startPos;
                float sqrDist = 0;
                for (int i = 0; i < _stepCount; i++)
                {
                    Vector3 newStr = str + (vel * t);
                    sqrDist += Vector3.SqrMagnitude(newStr - str);
                    Debug.DrawLine(str, newStr);
                    vel += new Vector3(ax, newAy, az) * t;
                    str = newStr;
                }
                Debug.Log($"sqrDist: {sqrDist}");
            }
            //else if (_start.position.y >= _end.position.y)
            //{
            //    newAy = (-b - Mathf.Sqrt(det) == 0) ? 0 : (-b - Mathf.Sqrt(det)) / (2 * a);
            //    float uy = ((2 * newAy * yDisp) - (newAy * newAy * n * n * t * t)) / (2 * newAy * n * t);
            //    //float yVel = uy;

            //    Vector3 vel = new Vector3(ux, uy, 0);
            //    Vector3 str = startPos;
            //    for (int i = 0; i < _stepCount; i++)
            //    {
            //        Vector3 newStr = str + (vel * t);
            //        Debug.DrawLine(str, newStr);
            //        vel += new Vector3(ax, newAy, 0) * t;
            //        str = newStr;
            //    }
            //}
        }
    }









    //[SerializeField] private int _stepCount;
    //[SerializeField] private float _stepDist;
    //[SerializeField] private float _timeStep;
    //[SerializeField] private Transform _start;
    //[SerializeField] private Transform _end;

    //private float A, B, C;
    //private float a, b, c, n, t, e, f, g, ay, ax;
    //[SerializeField] private float _sumDSquare;

    //private LineRenderer _lineRenderer;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _lineRenderer = GetComponent<LineRenderer>();
    //    _lineRenderer.positionCount = _stepCount;
    //}

    //private float CubeRoot(float num)
    //{
    //    float sign = Mathf.Sign(num);
    //    return sign * Mathf.Pow(Mathf.Abs(num), 1f / 3);
    //}
    //// Update is called once per frame
    //void Update()
    //{
    //    n = _stepCount;
    //    t = _timeStep /*Time.fixedDeltaTime*/ /*0.1f*/;
    //    ay = -Physics.gravity.y;
    //    ax = 0;
    //    //sumDSquare = (_stepDist * _stepDist) * n;

    //    float xDisp = _end.position.x - _start.position.x;
    //    float ux = xDisp / (_stepCount * t);
    //    float uy = ((2 * ay * (_end.position.y - _start.position.y)) - (ay * ay * n * n * t * t)) / (2 * ay * n * t);
    //    //float uy = (_end.position.y - _start.position.y)/ (_stepCount * Time.fixedDeltaTime);


    //    A = t * t * t * t * ((ax * ax) + (ay * ay));
    //    B = t * t * t * ((uy * ay) + (ux * ax));
    //    C = t * t * ((ux * ux) + (uy * uy));

    //    //A = (ax * ax) + (ay * ay);
    //    //B = (uy * ay) + (ux + ax);
    //    //C = (ux * ux) + (uy * uy);

    //    a = (2 * A) / 6;
    //    b = B - (3 * A / 6);
    //    c = C - B + (A / 6);

    //    //a = A/3;
    //    //b = B;
    //    //c = C;
    //    float d = -_sumDSquare;
    //    //_sumDSquare = (n * n * n * a) + (n * n * b) + (n * c);

    //    //float p = -b / (3 * a);
    //    //float q = (p * p * p) + (((b * c) - (3 * a * d)) / (6 * a * a));
    //    //float r = c / (3 * a);

    //    //float fh = (q * q) + Mathf.Pow(r - (p * p), 3);
    //    //float gh = Mathf.Pow(fh, 1f / 2);
    //    //float sign = Mathf.Sign(q - gh);
    //    //n = Mathf.Pow(q + gh, 1f / 3) + (sign * Mathf.Pow(Mathf.Abs(q - gh), 1f / 3)) + p;
    //    //Debug.Log("gh: " + gh + " n: " + n);

    //    // from mathologer's "500 years of not teaching the cubic formula" video
    //    float p = (c / a) - (b * b / (3 * a * a));
    //    float q = (2 * b * b * b / (27 * a * a * a)) - (b * c / (3 * a * a)) + (d / a);
    //    float det = Mathf.Pow(q / 2, 2f) + Mathf.Pow(p / 3, 3f);
    //    float sqrtDet = Mathf.Pow(det, 1f / 2);

    //    float tn = CubeRoot(-(q / 2) - sqrtDet) + CubeRoot(-(q / 2) + sqrtDet);
    //    Debug.Log("discriminant: " + det + " tn: " + tn /*+ " p: " + p + " q: " + q*/);


    //    //a = Gy;
    //    //b = Hy;
    //    //c = iy + (Gx * ux * ux) + (Hx * ux) + ix - sumDSquare;
    //    //float det = (b * b) - (4 * a * c);
    //    //Debug.Log("xVel: " + ux + " det: " + det);

    //    //float disp =  _start.position.y - _end.position.y;
    //    //float vuy = Mathf.Sqrt(Mathf.Abs(2 * ay * disp));
    //    //vuy *= Mathf.Sign(ay * disp);
    //    //float uy = (vuy) - (ay * n * t);
    //    //float det = 1;

    //    if (det < 0)
    //    {
    //        ////Debug.Log("complex root");
    //        ////return;
    //        ////float yVel = (-H + Mathf.Sqrt(-det) < 0) ? (-H + Mathf.Sqrt(-det)) / (2 * G) : (-H - Mathf.Sqrt(-det)) / (2 * G);
    //        //float yVel = (-b + Mathf.Sqrt(-det) < 0) ? (-b + Mathf.Sqrt(-det)) / (2 * a) : (-b - Mathf.Sqrt(-det)) / (2 * a);
    //        //Debug.Log("yVel: " + yVel);
    //        //Vector3 vel = new Vector3(ux, yVel, 0);
    //        //Vector3 str = _start.position;
    //        //for (int i = 0; i < _stepCount; i++)
    //        //{
    //        //    Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //        //    Debug.DrawLine(str, newStr);
    //        //    vel += -Physics.gravity * Time.fixedDeltaTime;
    //        //    str = newStr;
    //        //}
    //    }
    //    else
    //    {
    //        float yVel = uy;

    //        Vector3 vel = new Vector3(ux, yVel, 0);
    //        Vector3 str = _start.position;
    //        //int count = (int)Mathf.Abs(n);
    //        //int count = (int)(tn / _timeStep);
    //        float sqrDist = 0;
    //        float stepTime = tn / _stepCount;
    //        for (int i = 0; i < /*_stepCount*/ (int)tn /*_stepCount && stepTime > 0*/; i++)
    //        {
    //            Vector3 newStr = str + (vel * t /*stepTime*/);
    //            sqrDist += Vector3.SqrMagnitude(newStr - str);
    //            Debug.DrawLine(str, newStr);
    //            vel += new Vector3(ax, ay, 0) * t /*stepTime*/;
    //            str = newStr;
    //        }
    //        Debug.Log("sqrDist: " + sqrDist);
    //    }
    //}






    //[SerializeField] private int _stepCount;
    //[SerializeField] private float _stepDist;
    //[SerializeField] private float _ropeLength;
    //[SerializeField] private Transform _start;
    //[SerializeField] private Transform _end;

    //private float a, b, c, n, t, e, f, g, ay, ax;
    //private float sumDSquare;

    //private LineRenderer _lineRenderer;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _lineRenderer = GetComponent<LineRenderer>();
    //    _lineRenderer.positionCount = _stepCount;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    n = _stepCount;
    //    t = Time.fixedDeltaTime;
    //    ay = -Physics.gravity.y;
    //    ax = 0;
    //    sumDSquare = Mathf.Pow(_stepDist, 2) * n;

    //    //float len = Mathf.Pow(_start.position.y - _end.position.y, 2) + Mathf.Pow(_start.position.x - _end.position.x, 2);
    //    //sumDSquare = /*(len < _ropeLength * _ropeLength) ? len : */_ropeLength * _ropeLength;

    //    //float uy = ((2 * ay * (_end.position.y - _start.position.y)) - (ay * ay * n * n * t * t)) / (2 * ay * n * t);

    //    a = t * t;
    //    b = (n - 1) * t * ((6 * (_end.position.y - _start.position.y)) - (n * t * t * (n + n - 1))) / (3 * n);
    //    f = ((n * (n - 1) * ((2 * n) - 1)) / 6) * t * t * t * t * ((2 * (_end.position.y - _start.position.y) / (n * n * t * t)) + (ax * ax));

    //    float xDisp = _end.position.x - _start.position.x;
    //    float ux = xDisp / (_stepCount * Time.fixedDeltaTime);
    //    g = ux * ux * n * t * t;
    //    e = ux * ax * t * t * t * n * (n - 1);
    //    //e = Mathf.Pow(xDisp, 2);
    //    c = -sumDSquare + e + f + g /*- iy - ix*/;

    //    float det = (b * b) - (4 * a * c);
    //    //Debug.Log("xVel: " + ux + " det: " + det);
    //    //Debug.Log("sum D square: " + sumDSquare + " xdisp: " + xDisp  + " det: " + det);


    //    if (det < 0)
    //    {
    //        ////Debug.Log("complex root");
    //        ////return;
    //        ////float yVel = (-H + Mathf.Sqrt(-det) < 0) ? (-H + Mathf.Sqrt(-det)) / (2 * G) : (-H - Mathf.Sqrt(-det)) / (2 * G);
    //        //float yVel = (-b + Mathf.Sqrt(-det) < 0) ? (-b + Mathf.Sqrt(-det)) / (2 * a) : (-b - Mathf.Sqrt(-det)) / (2 * a);
    //        //Debug.Log("yVel: " + yVel);
    //        //Vector3 vel = new Vector3(ux, yVel, 0);
    //        //Vector3 str = _start.position;
    //        //for (int i = 0; i < _stepCount; i++)
    //        //{
    //        //    Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //        //    Debug.DrawLine(str, newStr);
    //        //    vel += -Physics.gravity * Time.fixedDeltaTime;
    //        //    str = newStr;
    //        //}
    //    }
    //    else
    //    {
    //        //float yVel = (-H + Mathf.Sqrt(det) < 0)? (-H + Mathf.Sqrt(det)) / (2 * G) : (-H - Mathf.Sqrt(det)) / (2 * G);
    //        ////Debug.Log("yVel: " + yVel);


    //        //float yVel = (-b + Mathf.Sqrt(det) < 0) ? (-b + Mathf.Sqrt(det)) / (2 * a) : (-b - Mathf.Sqrt(det)) / (2 * a);
    //        //float yVel = (sumDSquare - a - f - e) / b;

    //        //a = n * n * t * t;
    //        //b = ax * n * n * t * t * t * (n - 1);
    //        //f = ((/*(ay * ay) +*/ (ax * ax)) * n * n * t * t * t * t * (n - 1) * (n - 1)) / 4;

    //        //e = Mathf.Pow(_end.position.y - _start.position.y, 2);
    //        //c = -sumDSquare + e + f /*+ g*/ /*- iy - ix*/;

    //        //float det2 = (b * b) - (4 * a * c);
    //        //ux = (det2 >= 0) ? (-b + Mathf.Sqrt(det2)) / (2 * a) : ux;

    //        float yVel = (-b - Mathf.Sqrt(det)) / (2 * a);
    //        Debug.Log("yVel: " + yVel);
    //        //float yVel = uy;

    //        Vector3 vel = new Vector3(ux, yVel, 0);
    //        Vector3 str = _start.position;
    //        for (int i = 0; i < _stepCount; i++)
    //        {
    //            Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //            Debug.DrawLine(str, newStr);
    //            vel += -Physics.gravity * Time.fixedDeltaTime;
    //            str = newStr;
    //        }
    //    }
    //}




    //[SerializeField] private float length;
    ////[SerializeField] private float _xVel;
    //[SerializeField] private float yDiff;
    ////[SerializeField] private int _stepCount;
    //[SerializeField] private Transform start;
    //[SerializeField] private Transform end;

    //private float n, t, ay, ax, a, b, c, l;

    //private void Update()
    //{
    //    //n = _stepCount;
    //    //t = Time.fixedDeltaTime;
    //    //ay = -Physics.gravity.y;
    //    //ax = 0;

    //    yDiff = end.position.y - start.position.y;
    //    l = (end.position.x - start.position.x) / 2;

    //    a = 4 * ((length * length) - (yDiff * yDiff));
    //    b = 4 * length * ((yDiff * yDiff) - length);
    //    c = (length * length) + (yDiff * yDiff * ((4 * l * l) - (2 * length) + (yDiff * yDiff)));

    //    float det = (b * b) - (4 * a * c);

    //    //float uy = ((2 * ay * (_end.position.y - _start.position.y)) - (ay * ay * n * n * t * t)) / (2 * ay * n * t);

    //    //float ux = Mathf.Sqrt((_totalVel * _totalVel) - (uy * uy));

    //    //Vector3 vel = new Vector3(ux, uy, 0);
    //    //Vector3 str = _start.position;
    //    //for (int i = 0; i < _stepCount; i++)
    //    //{
    //    //    Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //    //    Debug.DrawLine(str, newStr);
    //    //    vel += -Physics.gravity * Time.fixedDeltaTime;
    //    //    str = newStr;
    //    //}


    //    if (det >= 0)
    //    {

    //        float H1 = (-b + Mathf.Sqrt(det)) / (2 * a);
    //        float y = Mathf.Sqrt((H1 * H1) - (l * l));
    //        //Debug.Log("y: " + y);

    //        float yIntPos = (start.position.y < end.position.y)? start.position.y - y : end.position.y - y;
    //        Vector3 inPos = new Vector3((start.position.x + end.position.x) / 2, yIntPos, start.position.z);
    //        Debug.DrawLine(start.position, inPos, Color.green);
    //        Debug.DrawLine(end.position, inPos, Color.green);


    //        H1 = (-b - Mathf.Sqrt(det)) / (2 * a);
    //        y = Mathf.Sqrt((H1 * H1) - (l * l));
    //        //Debug.Log("y: " + y);

    //        yIntPos = (start.position.y < end.position.y) ? start.position.y - y : end.position.y - y;
    //        inPos = new Vector3((start.position.x + end.position.x) / 2, yIntPos, start.position.z);
    //        Debug.DrawLine(start.position, inPos, Color.yellow);
    //        Debug.DrawLine(end.position, inPos, Color.yellow);
    //    }
    //}


    //[SerializeField] private int _stepCount;
    //[SerializeField] private Transform _start;
    //[SerializeField] private Transform _end;
    //[SerializeField] private float _sumDSquare;

    //private float a, b, c, n, t, e, f, g, ay, ax;

    //private LineRenderer _lineRenderer;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _lineRenderer = GetComponent<LineRenderer>();
    //    _lineRenderer.positionCount = _stepCount;

    //    //Debug.Log("epsilon: " + 2.7182818);
    //    //Debug.Log("log(2, e): " + Mathf.Log(2, 2.7182818f));
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    n = _stepCount;
    //    t = Time.fixedDeltaTime;
    //    ay = -Physics.gravity.y;
    //    ax = 1;

    //    //float uy = ((2 * ay * (_end.position.y - _start.position.y)) - (ay * ay * n * n * t * t)) / (2 * ay * n * t);
    //    float xDisp = _end.position.x - _start.position.x;
    //    //float ux = xDisp / (_stepCount * Time.fixedDeltaTime);
    //    float ux = ((2 * ax * xDisp) - (ax * ax * n * n * t * t)) / (2 * ax * n * t);

    //    float root = Mathf.Sqrt((ux * ux) + (2 * ax * _end.position.x)) - Mathf.Sqrt((ux * ux) + (2 * ax * _start.position.x));
    //    Debug.Log("root: " + root);
    //    float log = Mathf.Log(((ux * ux) + (2 * ax * _end.position.x)) / ((ux * ux) + (2 * ax * _start.position.x)), 2.7182818f);
    //    a = log / (2 * ax);
    //    b = (2 * root / ax) - (ay * ux * log / (ax * ax));
    //    Debug.Log("b: " + b);
    //    e = (_end.position.x - _start.position.x) * (1 + ((ay * ay) / (ax * ax)));
    //    f = ay * ay * ux * ux * log / (2 * ax * ax * ax);
    //    g = (-2) * ay * ux * root / (ax * ax);
    //    c = e + f + g - _sumDSquare;

    //    float det = (b * b) - (4 * a * c);
    //    Debug.Log("xVel: " + ux + " det: " + det);
    //    //Debug.Log("sum D square: " + sumDSquare + " xdisp: " + xDisp  + " det: " + det);


    //    if (det < 0)
    //    {
    //        ////Debug.Log("complex root");
    //        ////return;
    //        ////float yVel = (-H + Mathf.Sqrt(-det) < 0) ? (-H + Mathf.Sqrt(-det)) / (2 * G) : (-H - Mathf.Sqrt(-det)) / (2 * G);
    //        //float yVel = (-b + Mathf.Sqrt(-det) < 0) ? (-b + Mathf.Sqrt(-det)) / (2 * a) : (-b - Mathf.Sqrt(-det)) / (2 * a);
    //        //Debug.Log("yVel: " + yVel);
    //        //Vector3 vel = new Vector3(ux, yVel, 0);
    //        //Vector3 str = _start.position;
    //        //for (int i = 0; i < _stepCount; i++)
    //        //{
    //        //    Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //        //    Debug.DrawLine(str, newStr);
    //        //    vel += -Physics.gravity * Time.fixedDeltaTime;
    //        //    str = newStr;
    //        //}
    //    }
    //    else
    //    {
    //        float yVel = (-b + Mathf.Sqrt(det) == 0)? 0 : (-b + Mathf.Sqrt(det)) / (2 * a);
    //        Debug.Log("yVel: " + yVel);
    //        //float yVel = uy;

    //        Vector3 vel = new Vector3(ux, yVel, 0);
    //        Vector3 str = _start.position;
    //        for (int i = 0; i < _stepCount; i++)
    //        {
    //            Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //            Debug.DrawLine(str, newStr);
    //            vel += -Physics.gravity * Time.fixedDeltaTime;
    //            str = newStr;
    //        }
    //    }
    //}



    //[SerializeField] private int _stepCount;
    //[SerializeField] private Transform _start;
    //[SerializeField] private Transform _end;
    //[SerializeField] private float _sumDSquare;

    //private float a, b, c, n, t, e, f, g, ay, ax;

    //private LineRenderer _lineRenderer;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _lineRenderer = GetComponent<LineRenderer>();
    //    _lineRenderer.positionCount = _stepCount;

    //    //Debug.Log("epsilon: " + 2.7182818);
    //    //Debug.Log("log(2, e): " + Mathf.Log(2, 2.7182818f));
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    n = _stepCount;
    //    t = Time.fixedDeltaTime;
    //    ay = -Physics.gravity.y;

    //    //float uy = ((2 * ay * (_end.position.y - _start.position.y)) - (ay * ay * n * n * t * t)) / (2 * ay * n * t);
    //    float xDisp = _end.position.x - _start.position.x;
    //    float ux = xDisp / (_stepCount * Time.fixedDeltaTime);

    //    a = (xDisp) / (ux * ux);
    //    b = ay * ((_end.position.x * _end.position.x) - (_start.position.x * _start.position.x))/(ux * ux * ux);

    //    e = ay * ay * ((_end.position.x * _end.position.x * _end.position.x) - (_start.position.x * _start.position.x * _start.position.x)) / (3 * ux * ux * ux * ux);
    //    f = xDisp;
    //    c = e + f - _sumDSquare;

    //    float det = (b * b) - (4 * a * c);
    //    Debug.Log("xVel: " + ux + " det: " + det);
    //    //Debug.Log("sum D square: " + sumDSquare + " xdisp: " + xDisp  + " det: " + det);


    //    if (det < 0)
    //    {
    //        ////Debug.Log("complex root");
    //        ////return;
    //        ////float yVel = (-H + Mathf.Sqrt(-det) < 0) ? (-H + Mathf.Sqrt(-det)) / (2 * G) : (-H - Mathf.Sqrt(-det)) / (2 * G);
    //        //float yVel = (-b + Mathf.Sqrt(-det) < 0) ? (-b + Mathf.Sqrt(-det)) / (2 * a) : (-b - Mathf.Sqrt(-det)) / (2 * a);
    //        //Debug.Log("yVel: " + yVel);
    //        //Vector3 vel = new Vector3(ux, yVel, 0);
    //        //Vector3 str = _start.position;
    //        //for (int i = 0; i < _stepCount; i++)
    //        //{
    //        //    Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //        //    Debug.DrawLine(str, newStr);
    //        //    vel += -Physics.gravity * Time.fixedDeltaTime;
    //        //    str = newStr;
    //        //}
    //    }
    //    else
    //    {
    //        float yVel = (-b - Mathf.Sqrt(det) == 0) ? 0 : (-b - Mathf.Sqrt(det)) / (2 * a);
    //        Debug.Log("yVel: " + yVel);
    //        //float yVel = uy;

    //        Vector3 vel = new Vector3(ux, yVel, 0);
    //        Vector3 str = _start.position;
    //        for (int i = 0; i < _stepCount; i++)
    //        {
    //            Vector3 newStr = str + (vel * Time.fixedDeltaTime);
    //            Debug.DrawLine(str, newStr);
    //            vel += -Physics.gravity * Time.fixedDeltaTime;
    //            str = newStr;
    //        }
    //    }
    //}

}
