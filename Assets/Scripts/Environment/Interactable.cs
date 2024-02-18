using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public static Action<Interactable> InteractableCreated;
    public static Action<Interactable> InteractableDestroyed;
    public bool ShowPromptHandle;
    public Vector3 PromptPos;

    protected virtual void Start()
    {
        InteractableCreated.Invoke(this);
    }

    private void OnDisable()
    {
        InteractableDestroyed.Invoke(this);
    }

    public abstract void Interact();
}

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    private Interactable _interactable;
    protected virtual void OnEnable()
    {
        _interactable = (Interactable)target;
    }

    protected virtual void OnSceneGUI()
    {
        if (_interactable.ShowPromptHandle)
        {
            _interactable.PromptPos = Handles.PositionHandle(_interactable.PromptPos, Quaternion.identity);
            Handles.DrawWireCube(_interactable.PromptPos, Vector3.one * 0.5f);
        }
        
    }
}
