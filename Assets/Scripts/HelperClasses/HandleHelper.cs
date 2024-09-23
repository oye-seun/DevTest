using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HandleHelper : MonoBehaviour
{
    public static void PosFwdHandle_AxisUp(Vector3 pos, Vector3 fwd, out Vector3 newPos, out Vector3 newFwd)
    {
        float angle = /*(fwd.x >= 0) ? Mathf.Atan2(fwd.x, fwd.z) : Mathf.PI +*/ /*Mathf.Sign(fwd.z) **/ Mathf.Atan2(fwd.x, fwd.z);
        Quaternion fwdRot = Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0);
        Quaternion updatedFwdRot = Handles.RotationHandle(fwdRot, pos);
        newPos = Handles.PositionHandle(pos, fwdRot);
        //float tanEulerY = (updatedFwdRot.eulerAngles.y > 180) ? -Mathf.Tan(updatedFwdRot.eulerAngles.y * Mathf.Deg2Rad) : Mathf.Tan(updatedFwdRot.eulerAngles.y * Mathf.Deg2Rad);
        float newAngle = updatedFwdRot.eulerAngles.y;
        float z = 1;
        if (updatedFwdRot.eulerAngles.y > 90 && updatedFwdRot.eulerAngles.y < 270)
        {
            newAngle = 180 - updatedFwdRot.eulerAngles.y;
            z = -1;
        }
        newFwd = new Vector3(Mathf.Tan(newAngle * Mathf.Deg2Rad) * 1, 0, z).normalized;
    }
}
