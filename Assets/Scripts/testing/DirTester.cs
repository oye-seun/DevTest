using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirTester : MonoBehaviour
{
    public GameObject point1;
    public GameObject point2;

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = point1.transform.position - point2.transform.position;
        if (dir.magnitude > 1f) dir.Normalize();
        //move = transform.InverseTransformDirection(move);
        dir = Vector3.ProjectOnPlane(dir, Vector3.up);

        Vector3 turn = transform.localEulerAngles;
        turn.y = Mathf.Atan2(Mathf.Abs(dir.x), Mathf.Abs(dir.z)) * Mathf.Rad2Deg;

        Debug.Log(dir);
        // quadrant corrections
        if (dir.x < 0 && dir.z > 0) { Debug.Log("2nd"); turn.y = 360-turn.y; }
        else if (dir.x < 0 && dir.z < 0) { Debug.Log("3rd"); turn.y = 180 + turn.y; }
        else if (dir.x > 0 && dir.z < 0) { Debug.Log("4th"); turn.y = 180 - turn.y; }


        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, turn, 10 * Time.deltaTime);
    }
}
