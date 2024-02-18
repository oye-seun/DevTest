using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System;

public class PuzzleRockSlider : MonoBehaviour
{
    [SerializeField] private Vector3 _max;
    [SerializeField] private Vector3 _min;
    public bool set { get; private set; }

    public void Set(float dur, Action onFinished)
    {
        set = true;
        transform.DOMove(_min, dur).OnComplete(() => { onFinished(); });
    }

    public void ReSet(float dur, Action onFinished)
    {
        set = false;
        transform.DOMove(_max, dur).OnComplete(() => { onFinished(); });
    }

    #region Editor tools
    #if UNITY_EDITOR
    public void SetMinPos()
    {
        _min = transform.position;
    }

    public void SetMaxPos()
    {
        _max = transform.position;
    }
    #endif
    #endregion
}

[CustomEditor(typeof(PuzzleRockSlider))]
public class PuzzleRockSliderEditor : Editor
{
    PuzzleRockSlider slider;
    private void OnEnable()
    {
        slider = (PuzzleRockSlider)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Set Min")) slider.SetMinPos();
        if(GUILayout.Button("Set Max")) slider.SetMaxPos();
        EditorGUILayout.EndHorizontal();
    }
}