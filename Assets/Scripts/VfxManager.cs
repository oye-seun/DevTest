using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

//public enum Vfxs {}
public class VfxManager : MonoSingleton<VfxManager>
{
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void PlayVfx(GameObject vfxPrefab, Vector3 position, Quaternion rotation, float duration)
    {
         GameObject vfx = Instantiate(vfxPrefab, position, rotation, transform);
        Destroy(vfx, duration);
        //StartCoroutine(DestroyVfxOnEnd(vfx));
    }

    //private IEnumerator DestroyVfxOnEnd(GameObject vfx)
    //{
    //    VisualEffect effect = vfx.GetComponent<VisualEffect>();
    //    float particleCount = 0;
    //    while (!(effect.aliveParticleCount - particleCount < 0 && !effect.HasAnySystemAwake()))
    //    {
    //        Debug.Log("particle count: " + effect.aliveParticleCount + " particle ");
    //        yield return null;
    //    }

    //    Destroy(vfx);
    //}
}

