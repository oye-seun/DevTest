using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerData : MonoBehaviour
{
    public static int coins = 0;
    public static List<InventoryObject> inventory = new List<InventoryObject>();
    public static Transform leftHandTarget;
    public static Transform rightHandTarget;

    public static TwoBoneIKConstraint leftHandConstraint;
    public static TwoBoneIKConstraint rightHandConstraint;

    private void Start()
    {
        leftHandTarget = transform.Find("Rig 1").Find("left hand aim").Find("target").GetComponent<Transform>();
        rightHandTarget = transform.Find("Rig 1").Find("right hand aim").Find("target").GetComponent<Transform>();

        leftHandConstraint = transform.Find("Rig 1").Find("left hand aim").GetComponent<TwoBoneIKConstraint>();
        rightHandConstraint = transform.Find("Rig 1").Find("right hand aim").GetComponent<TwoBoneIKConstraint>();
    }
}
