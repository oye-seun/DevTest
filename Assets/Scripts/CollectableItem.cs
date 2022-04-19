using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : objectclass, IObject
{
    private GameObject sparkle;
    private Camera cam;
    private Vector3 initialScale;
    public float speed = 2;
    public float size = 0.5f;

    private Tributton[] options = new Tributton[1];
    public Vector3 adjustedPosition;

    [SerializeReference]
    public List<InventoryObject> item;


    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectsOfType<Camera>()[0];
        sparkle = transform.Find("sparkle").gameObject;
        sparkle.transform.LookAt(cam.transform);
        sparkle.transform.localEulerAngles = new Vector3(sparkle.transform.localEulerAngles.x, cam.transform.eulerAngles.y, sparkle.transform.localEulerAngles.z);

        initialScale = sparkle.transform.localScale;

        options[0] = new Tributton("Pick", () => { PickItem(); });

        if(adjustedPosition == null)
        {
            adjustedPosition = new Vector3(transform.position.x + 2, transform.position.y + 2, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // scale in and out
        sparkle.transform.localScale = initialScale + (Vector3.one * (size * Mathf.Cos(Time.time * speed)));
    }

    public void setUI()
    {
        GameManager.UISetter(options, adjustedPosition);

        UIPosDerender();
    }

    public void PickItem()
    {
        GameManager.UIDesetter();
        GameManager.RemovePosEventByTag("options");
        Destroy(gameObject);
        GameManager.OpenCrateUI(item);
    }
}
