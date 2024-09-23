using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerData : MonoBehaviour
{
    public int coins { get; private set; } = 0;
    public Dictionary<string, InventoryObject> inventory { get; private set; } = new Dictionary<string, InventoryObject>();
    [SerializeField] private Transform _leftHandTarget;
    [SerializeField] private Transform _rightHandTarget;
    [SerializeField] private TwoBoneIKConstraint _leftHandConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightHandConstraint;

    public static Transform leftHandTarget { get; private set; }
    public static Transform rightHandTarget { get; private set; }

    public static TwoBoneIKConstraint leftHandConstraint { get; private set; }
    public static TwoBoneIKConstraint rightHandConstraint { get; private set; }

    private void Start()
    {
        //leftHandTarget = transform.Find("Rig 1").Find("left hand aim").Find("target").GetComponent<Transform>();
        //rightHandTarget = transform.Find("Rig 1").Find("right hand aim").Find("target").GetComponent<Transform>();

        //leftHandConstraint = transform.Find("Rig 1").Find("left hand aim").GetComponent<TwoBoneIKConstraint>();
        //rightHandConstraint = transform.Find("Rig 1").Find("right hand aim").GetComponent<TwoBoneIKConstraint>();


        leftHandTarget = _leftHandTarget;
        rightHandTarget = _rightHandTarget;

        leftHandConstraint = _leftHandConstraint;
        rightHandConstraint = _rightHandConstraint;
    }
}
