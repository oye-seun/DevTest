using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("maintheme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, T => T.name == name);
        if (s != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogError("Play() failed. Couldn't find sound with name: " + name);
        }
    }

    public void PauseAll()
    {
        foreach(Sound s in sounds)
        {
            if (s.source.isPlaying)
            {
                s.source.Pause();
                s.paused = true;
            }
        }
    }

    public void ResumeAll()
    {
        foreach (Sound s in sounds)
        {
            if (s.paused)
            {
                s.source.UnPause();
                s.paused = false;
            }
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, T => T.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogError("Stop() failed. Couldn't find sound with name: " + name);
        }
    }
}


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume;
    [Range(-3f, 3)]
    public float pitch = 1;

    public bool loop;

    [HideInInspector]
    public bool paused;

    [HideInInspector]
    public AudioSource source;
}
