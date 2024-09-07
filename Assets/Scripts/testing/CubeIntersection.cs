using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CubeIntersection : MonoBehaviour
{
    public Transform raycaster;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 intersecPos = new Vector3();
        CubeIntersection_float(transform.position, transform.localScale, raycaster.position, raycaster.forward, out intersecPos);
        Debug.DrawLine(raycaster.position, intersecPos, Color.yellow);
    }


    Vector3 TransLerp(Vector3 minOrg, Vector3 maxOrg, Vector3 valOrg, Vector3 minNew, Vector3 maxNew)
    {
        return minNew + DivideVectors(MultiplyVectors((maxNew - minNew), (valOrg - minOrg)), (maxOrg - minOrg));
    }

    void CubeIntersection_float(Vector3 cubePos, Vector3 cubeSize, Vector3 rayOrigin, Vector3 rayDirection, out Vector3 intersection)
    {
        rayOrigin = TransLerp(cubePos - cubeSize / 2, cubePos + cubeSize / 2, rayOrigin, -cubeSize / 2, cubeSize / 2);

        rayDirection = rayDirection.normalized;
        Vector3 absRayDir = new Vector3(Mathf.Abs(rayDirection.x), Mathf.Abs(rayDirection.y), Mathf.Abs(rayDirection.z)); //abs(rayDirection)
        Vector3 antiAbs = DivideVectors(rayDirection, absRayDir);

        Vector3 stepVec = DivideVectors((MultiplyVectors(antiAbs, cubeSize / 2) - rayOrigin), rayDirection); 

        float stepMin = Mathf.Min(stepVec.x, stepVec.y);
        stepMin = Mathf.Min(stepMin, stepVec.z);

        Debug.Log("StepVec: " + stepVec + "  min: " + stepMin);
        intersection = TransLerp(-cubeSize / 2, cubeSize / 2, rayOrigin + (rayDirection * stepMin), cubePos - cubeSize / 2, cubePos + cubeSize / 2);
    }


    private Vector3 MultiplyVectors(Vector3 A, Vector3 B)
    {
        return new Vector3(A.x * B.x, A.y * B.y, A.z * B.z);
    }


    private Vector3 DivideVectors(Vector3 A, Vector3 B)
    {
        return new Vector3(A.x / B.x, A.y / B.y, A.z / B.z);
    }
}
