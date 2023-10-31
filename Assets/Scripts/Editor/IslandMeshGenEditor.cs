using System.Collections;
#if (UNITY_EDITOR)

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IslandMeshGen))]
public class IslandMeshGenEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        IslandMeshGen meshGen = (IslandMeshGen)target;
        if (DrawDefaultInspector())
        {
            if (meshGen.AutoUpdate) meshGen.GenerateMesh();
        }


        if (GUILayout.Button("Generate Map"))
        {
            meshGen.GenerateMesh();
        }
    }
}
#endif
