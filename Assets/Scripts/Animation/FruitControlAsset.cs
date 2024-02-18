using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FruitControlAsset : PlayableAsset
{
    //public ExposedReference<GameObject> gameObject;
    public FruitControlBehaviour template;
    //public Vector3 endPos;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<FruitControlBehaviour>.Create(graph, template);
        //FruitControlBehaviour fruit = playable.GetBehaviour();
        //fruit.fruit = gameObject.Resolve(graph.GetResolver());
        //fruit.endPos = endPos;
        return playable;
    }
}
