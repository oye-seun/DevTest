using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public void FaceCam()
    {
        transform.LookAt(FindObjectOfType<Camera>().gameObject.transform);
        transform.forward = -transform.up;
    }
}

