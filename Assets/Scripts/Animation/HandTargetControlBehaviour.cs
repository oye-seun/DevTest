using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public enum HandTargetModes
{
    Pos, Rot, Pos_Rot, CalcRot, Pos_CalcRot
}
public class HandTargetControlBehaviour : PlayableBehaviour
{
    public HandTargetModes mode;
    public Vector3 position;
    public Quaternion rotation;
    public GameObject handTarget;
    public GameObject targetObject;
    public GameObject player;


    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        switch (mode)
        {
            case HandTargetModes.Pos:
                handTarget.transform.position = position;
                break;
            case HandTargetModes.Rot:
                handTarget.transform.rotation = rotation;
                break;
            case HandTargetModes.Pos_Rot:
                handTarget.transform.position = position;
                handTarget.transform.rotation = rotation;
                break;
            case HandTargetModes.CalcRot:
                CalcRot();
                break;
            case HandTargetModes.Pos_CalcRot:
                handTarget.transform.position = position;
                CalcRot();
                break;
        }


        
        //else
        //{
        //    Debug.Log("null references");
        //}
    }

    private void CalcRot()
    {
        if (handTarget != null && targetObject != null)
        {
            Vector3 fwd = (player.transform.position - targetObject.transform.position).normalized;
            //handTarget.transform.forward = /*rotation **/ fwd;
            //Debug.Log(" PreHTfwd: " + handTarget.transform.forward);
            handTarget.transform.forward = rotation * fwd;
            //Debug.Log("fwd: " + fwd + " HTfwd: " + handTarget.transform.forward);
        }
    }
}
