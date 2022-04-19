using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BillBoard))]
public class RotateBillBoard : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BillBoard device = (BillBoard)target;
        if (GUILayout.Button("Set Billboard"))
        {
            device.FaceCam();
        }
    }
}
