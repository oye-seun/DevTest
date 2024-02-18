using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(FruitControlAsset))]
[TrackBindingType(typeof(GameObject))]
public class FruitControlTrack : TrackAsset 
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<FruitControlMixerBehaviour>.Create(graph, inputCount);
    }
}
