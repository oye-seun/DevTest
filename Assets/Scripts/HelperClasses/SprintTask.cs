using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintTask : MonoBehaviour
{
    public delegate void RunTemplate(float val);

    public static IEnumerator Run_c(float min, float max, float duration, RunTemplate runFunc, Action OnCompleted)
    {
        float time = 0;
        while (time < duration)
        {
            runFunc(Mathf.Lerp(min, max, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        runFunc(Mathf.Lerp(min, max, 1));
        OnCompleted();
    }
}
