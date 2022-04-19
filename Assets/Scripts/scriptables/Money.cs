using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable]
[CreateAssetMenu(fileName = "New money", menuName = "inventoryobject/money")]
public class Money : InventoryObject
{
    public new string name = "100 coins";
    public Sprite icon;
    public string about = "100 coins";
    public int value = 100;

    public override string Name { get { return name; } }

    public override Sprite Icon { get { return icon; } }

    public override string About { get { return about; } }

    public override void OnCollect()
    {
        Debug.Log("collected coin");
        PlayerData.coins += value;
        GameManager.SetCoins(PlayerData.coins);
    }

    public override void OnUse()
    {
        Debug.Log("used coin");
    }
}
