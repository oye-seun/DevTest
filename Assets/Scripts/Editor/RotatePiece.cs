using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ActivationDevice))]
public class RotatePiece : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ActivationDevice device = (ActivationDevice)target;
        if(GUILayout.Button("Rotate Piece"))
        {
            device.SimpleRotateCentre();
        }
    }
}
