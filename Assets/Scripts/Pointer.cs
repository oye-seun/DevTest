using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private GameObject pointer;
    private GameObject ring;

    public float pointerMid;
    public float speed = 1;
    public float amplitude = 0.5f;
    public float FloorOffset = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        pointer = transform.Find("pointer").gameObject;
        ring = transform.Find("ring").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        pointer.transform.position = new Vector3(pointer.transform.position.x, pointerMid + (Mathf.Cos(Time.time*speed)*amplitude), pointer.transform.position.z);
    }

    public void SetPosiiton(Vector3 pos, Vector3 normal)
    {
        pos.y += FloorOffset;
        transform.position = pos;
        //ring.transform.rotation = Quaternion.FromToRotation(ring.transform.up, normal);  //bend ring according to plane normal does not work very well
        pointer.transform.eulerAngles = new Vector3(pointer.transform.eulerAngles.x, Quaternion.LookRotation(-GameManager.cam.transform.forward).eulerAngles.y, pointer.transform.eulerAngles.z);
    }

    public void Hide()
    {
        if (canHide)
        {
            pointer.GetComponent<MeshRenderer>().enabled = false;
            ring.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private bool canHide = true;
    private bool EHATrun;
    public void Show(float maxtime)
    {
        pointer.GetComponent<MeshRenderer>().enabled = true;
        ring.GetComponent<MeshRenderer>().enabled = true;
        canHide = false;
        if (!EHATrun)
        {
            StartCoroutine(EnableHideAfterTime(maxtime));
        }
    }

    public IEnumerator EnableHideAfterTime(float maxtime)
    {
        EHATrun = true;
        float time = 0;
        while(time < maxtime)
        {
            //do nothing
            time += Time.deltaTime;
            yield return null;
        }

        // show pointer
        canHide = true;
        EHATrun = false;
    }
}
