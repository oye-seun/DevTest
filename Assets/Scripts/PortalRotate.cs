using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRotate : MonoBehaviour
{
    private float speed = 0;
    private Material mat;
    private ParticleSystem particle1, particle2;
    public Light portalLight;

    public AudioSource portalStart;
    public AudioSource portalRun;

    private bool portalStartPaused;
    private bool portalRunPaused;

    private void Start()
    {
        mat = transform.parent.GetComponent<MeshRenderer>().sharedMaterial;
        mat.SetFloat("_visible", 0);
        particle1 = transform.Find("particle1").GetComponent<ParticleSystem>();
        particle2 = transform.Find("particle2").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }


    public IEnumerator StartPortal(float mintime, float maxtime)
    {

        portalStart.Play();
        bool switchedsound = false;

        float time = mintime;
        while(time < maxtime)
        {
            speed = Mathf.Clamp((Environment.interpolate(mintime, maxtime, time*time, 0, 1000)),0f ,1000f);

            ParticleSystem.EmissionModule emission1 = particle1.emission;
            ParticleSystem.EmissionModule emission2 = particle2.emission;

            emission1.rateOverTime = Environment.interpolate(mintime, maxtime, time, 0, 500);
            emission2.rateOverTime = Environment.interpolate(mintime, maxtime, time, 0, 500);

            portalLight.intensity = Environment.interpolate(mintime, maxtime, time, 0, 50);

            if(!switchedsound && time >= 5.5f)
            {
                portalStart.Stop();
                portalRun.Play();

                StartCoroutine(ShowPortal(0, 0.5f));
                switchedsound = true;
            }

            time += Time.deltaTime;
            yield return null;
        }
        transform.parent.GetComponent<Collider>().enabled = true;
    }

    public IEnumerator ShowPortal(float mintime, float maxtime)
    {
        float time = mintime;
        while (time < maxtime)
        {
            mat.SetFloat("_visible", Environment.interpolate(mintime, maxtime, time, 0, 1));
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void PauseSounds()
    {
        if (portalStart.isPlaying)
        {
            portalStart.Pause();
            portalStartPaused = true;
        }

        if (portalRun.isPlaying)
        {
            portalRun.Pause();
            portalRunPaused = true;
        }
    }

    public void ResumeSounds()
    {
        if (portalStartPaused)
        {
            portalStart.UnPause();
            portalStartPaused = false;
        }

        if (portalRunPaused)
        {
            portalRun.UnPause();
            portalRunPaused = false;
        }
    }
}
