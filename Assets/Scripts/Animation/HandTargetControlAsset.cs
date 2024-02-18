using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Playables;

public class HandTargetControlAsset : PlayableAsset
{
    public HandTargetModes modes;
    public Vector3 position;
    public Quaternion rotation;
    public ExposedReference<GameObject> handTarget;
    public ExposedReference<GameObject> targetObject;
    public ExposedReference<GameObject> player;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<HandTargetControlBehaviour>.Create(graph);

        var handTargetBehaviour = playable.GetBehaviour();
        handTargetBehaviour.handTarget = handTarget.Resolve(graph.GetResolver());
        handTargetBehaviour.targetObject = targetObject.Resolve(graph.GetResolver());
        handTargetBehaviour.player = player.Resolve(graph.GetResolver());
        handTargetBehaviour.rotation = rotation;
        handTargetBehaviour.position = position;
        handTargetBehaviour.mode = modes;
        return playable;
    }
}
