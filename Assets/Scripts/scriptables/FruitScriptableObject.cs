using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFruit", menuName = "Inventory Object/Fruit")]
public class FruitScriptableObject : InventoryObject
{
    [SerializeField] private string _name = "Energy Fruit";
    [SerializeField] private string _about = "Holds Energy that can be used to actuate specific devices";

    public override string Name { get { return _name; } }
    public override string About { get { return _about; } }
}
