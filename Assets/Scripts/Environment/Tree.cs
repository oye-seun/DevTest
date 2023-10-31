using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tree : Interactable
{
    public override void Interact()
    {
        throw new System.NotImplementedException();
    }
}


[CustomEditor(typeof(Tree))]
public class TreeEditor : InteractableEditor
{
    protected override void OnSceneGUI()
    {
        base.OnSceneGUI();
    }
}