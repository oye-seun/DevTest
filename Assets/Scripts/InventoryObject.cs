using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InventoryObject : ScriptableObject
{
    public virtual string Name { get; }
    public virtual Sprite Icon { get; }
    public virtual string About { get; }
    public virtual void OnCollect() { }
    public virtual void OnUse() { }
}
