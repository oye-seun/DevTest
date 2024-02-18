using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class FruitControlBehaviour : PlayableBehaviour
{
    //public GameObject fruit = null;
    public Vector3 endPos;
    public bool useLocal;
    //public float intensity = 1f;

    //public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    //{
    //    if (fruit != null)
    //    {
    //        //light.color = color;
    //        //light.intensity = intensity;
    //        //Debug.Log("info effWeight: " + info.e);
    //        fruit.transform.position = Vector3.Lerp(Vector3.zero, endPos, info.weight);
    //    }
    //    else
    //    {
    //        fruit = playerData as GameObject;
    //        Debug.Log("fruit null");
    //    }
    //}
}
