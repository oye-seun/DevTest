using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New keypiece", menuName = "inventoryobject/keypiece")]
public class KeyPiece : InventoryObject
{
    public new string name = "broken key piece";
    public Sprite icon;
    public string about = "this piece is needed to complete the broken key";

    public override string Name { get { return name; } }
    public override Sprite Icon { get { return icon; } }
    public override string About { get { return about; } }

    public override void OnCollect()
    {
        //Debug.Log("collected key");
        PlayerData.inventory.Add(this);
    }

    public override void OnUse()
    {
        //Debug.Log("use key");
    }
}
