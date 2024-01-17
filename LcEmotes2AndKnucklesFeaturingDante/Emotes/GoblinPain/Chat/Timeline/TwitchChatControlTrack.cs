using System;
using LcEmotes2AndKnucklesFeaturingDante.Utils;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat.Timeline;

[TrackClipType(typeof(TwitchChatControlAsset))]
[TrackBindingType(typeof(TwitchChatController))]
public class TwitchChatControlTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var director = go.GetComponent<PlayableDirector>();
        var binding = director.GetGenericBinding(this);
        
        if (binding is not TwitchChatController controller)
            return ScriptPlayable<TwitchChatControllerMixerBehaviour>.Create(graph, inputCount);

        foreach (var clip in GetClips())
        {
            if (clip.asset is not TwitchChatControlAsset chatControlAsset)
                continue;

            chatControlAsset.messages = controller.messages.Copy();

            if (chatControlAsset.weights.Length != controller.weights.Length)
                Array.Resize(ref chatControlAsset.weights, controller.weights.Length);
        }
        
        return ScriptPlayable<TwitchChatControllerMixerBehaviour>.Create(graph, inputCount);
    }
}