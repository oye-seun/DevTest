using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SplineTester : MonoBehaviour
{
    [SerializeField] private PlayerPositioner.Waypoint _point1;
    [SerializeField] private PlayerPositioner.Waypoint _point2;

    private void Start()
    {
        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SplineTester))]
public class SplineTesterEditor : Editor
{
    private SerializedProperty _point1;
    private SerializedProperty _point2;
    private void OnEnable()
    {
        _point1 = serializedObject.FindProperty("_point1");
        _point2 = serializedObject.FindProperty("_point2");
    }

    private void OnSceneGUI()
    {
        // control handles
        Vector3 pos = _point1.FindPropertyRelative("Position").vector3Value;
        Vector3 fwd = _point1.FindPropertyRelative("Forward").vector3Value;
        Vector3 newPos = Vector3.zero;
        Vector3 newFwd = Vector3.zero;
        HandleHelper.PosFwdHandle_AxisUp(pos, fwd, out newPos, out newFwd);
        _point1.FindPropertyRelative("Position").vector3Value = newPos;
        _point1.FindPropertyRelative("Forward").vector3Value = newFwd;


        Vector3 pos2 = _point2.FindPropertyRelative("Position").vector3Value;
        Vector3 fwd2 = _point2.FindPropertyRelative("Forward").vector3Value;
        Vector3 newPos2 = Vector3.zero;
        Vector3 newFwd2 = Vector3.zero;
        HandleHelper.PosFwdHandle_AxisUp(pos2, fwd2, out newPos2, out newFwd2);
        _point2.FindPropertyRelative("Position").vector3Value = newPos2;
        _point2.FindPropertyRelative("Forward").vector3Value = newFwd2;
        serializedObject.ApplyModifiedProperties();

        // Draw spline
        float point1fwdStrength = _point1.FindPropertyRelative("FwdStrength").floatValue;
        float point2fwdStrength = _point2.FindPropertyRelative("FwdStrength").floatValue;
        Spline spline = new Spline(newPos, newFwd * point1fwdStrength, newPos2, newFwd2 * point2fwdStrength);

        for(int i = 0; i < 50; i++)
        {
            Vector3 t1pos = spline.GetPosAt((float)i / 50);
            t1pos.y = newPos.y;
            Vector3 t2pos = spline.GetPosAt((float)(i + 1) / 50);
            t2pos.y = newPos.y;
            Debug.DrawLine(t1pos, t2pos, Color.yellow);
        }
    }
}
#endif