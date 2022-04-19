using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class floater : MonoBehaviour
{
    public float offset;
    public float MaxDisplacement;
    public float scale = 1f;
    public float loadedDip = -0.9f;
    private float dip;
    private Vector3 centrepos;
    private ParticleSystem particle;
    ParticleSystem.EmissionModule emission;

    private void Awake()
    {
        transform.tag = "floatable";
        centrepos = transform.position;
        particle = transform.Find("particles").GetComponent<ParticleSystem>();
        emission = particle.emission;
        emission.rateOverTime = 0;
    }


    private void FixedUpdate()
    {
        transform.position = new Vector3(centrepos.x, centrepos.y + (Mathf.Cos(Time.time * scale + offset) * MaxDisplacement) + dip, centrepos.z);
    }

    public void loaded()
    {
        // exeprimental way to interpolate float values, instead of using Environment.interpolate
        DOTween.To(() => dip, x => dip = x, loadedDip, 0.9f).OnComplete(()=> {
            DOTween.To(() => dip, x => dip = x, 0, 3f);
        });
        //Debug.Log("loaded");
        StartCoroutine(emitparticle(10,Random.Range(0.5f,1)));
    }

    public IEnumerator emitparticle(float rateovertime, float maxtime)
    {
        float time = 0;
        emission.rateOverTime = rateovertime;
        while(time < maxtime)
        {
            //do nothing
            time += Time.deltaTime;
            yield return null;
        }
        emission.rateOverTime = 0;
    }
}
