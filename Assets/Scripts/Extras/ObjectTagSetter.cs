using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectTagSetter : MonoBehaviour
{
    public void Awake()
    {
        gameObject.tag = "object";
    }
}
