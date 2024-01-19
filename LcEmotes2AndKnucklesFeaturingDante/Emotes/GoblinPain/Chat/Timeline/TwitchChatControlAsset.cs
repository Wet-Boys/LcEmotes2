using LcEmotes2AndKnucklesFeaturingDante.Utils;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat.Timeline;

public class TwitchChatControlAsset : PlayableAsset, ITimelineClipAsset
{
    public float[] weights = [];
    public string[] messages = [];
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TwitchChatControllerBehaviour>.Create(graph);

        var controllerBehaviour = playable.GetBehaviour();

        controllerBehaviour.weights = weights.Copy();

        return playable;
    }

    public ClipCaps clipCaps => ClipCaps.Blending;
}