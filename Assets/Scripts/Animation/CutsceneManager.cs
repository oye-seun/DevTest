using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : MonoSingleton<CutsceneManager>
{
    public static event Action CutsceneEnded;

    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private List <TimelineAsset> _clips;
    private Action _onComplete;


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnEnable()
    {
        _playableDirector.stopped += OnCompleted;
    }

    private void OnDisable()
    {
        _playableDirector.stopped -= OnCompleted;
    }

    public void PlayClip(int num, Action onComplete)
    {
        _onComplete = onComplete;
        _playableDirector.playableAsset = _clips[num];
        _playableDirector.Play();
    }

    private void OnCompleted(PlayableDirector director)
    {
        CutsceneEnded?.Invoke();
        _onComplete();
    }
}
