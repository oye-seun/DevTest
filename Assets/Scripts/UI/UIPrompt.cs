using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPrompt : MonoSingleton<UIPrompt>
{
    [SerializeField] private GameObject _prompts;
    [SerializeField] private GameObject _back;

    public void ShowPrompt(bool state)
    {
        _prompts.SetActive(state);
    }

    public void ShowBackPrompt(bool state)
    {
        if(state) _prompts.SetActive(state);
        _back.SetActive(state);
    }
}
