using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraControl : MonoBehaviour
{
    private static Camera cam;
    private static Transform camtrans;

    void Start()
    {
        cam = this.GetComponent<Camera>();
        camtrans = transform;
    }

    public static void MoveCam(CamState state, float time, Generalevent.PressButton onfinished)
    {
        cam.DOFieldOfView(state.FOV, time);
        camtrans.DOMove(state.pos, time);
        camtrans.DOLocalRotate(state.rot, time).OnComplete(() => { onfinished(); });
    }
}

[SerializeField]
public class CamState
{
    public float FOV;
    public Vector3 pos;
    public Vector3 rot;

    public CamState(float fov, Vector3 Pos, Vector3 Rot)
    {
        FOV = fov;
        pos = Pos;
        rot = Rot;
    }
}
