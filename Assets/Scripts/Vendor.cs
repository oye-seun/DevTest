using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : objectclass, IObject
{
    private Tributton[] options = new Tributton[1];
    private Vector3 adjustedPosition;

    private void Start()
    {
        options[0] = new Tributton("buy", () => { Debug.Log("buy stuff"); GameManager.ShowInfo("unfortunately this option is not yet available"); });

        adjustedPosition = new Vector3(transform.position.x + 2, transform.position.y + 2, transform.position.z);
    }

    public void setUI()
    {
        GameManager.UISetter(options, adjustedPosition);

        UIPosDerender();
    }
}
