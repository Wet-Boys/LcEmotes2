using System;
using UnityEngine;
using UnityEngine.Playables;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

public class TwitchChatControlAsset : PlayableAsset
{
    // ReSharper disable once InconsistentNaming
    public ExposedReference<TwitchChatController> controller;
    
    public float[] weights = [];
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TwitchChatControllerBehaviour>.Create(graph);

        var controllerBehaviour = playable.GetBehaviour();
        controllerBehaviour.controller = controller.Resolve(graph.GetResolver());
        
        if (weights.Length != controllerBehaviour.controller.weights.Length)
            Array.Resize(ref weights, controllerBehaviour.controller.weights.Length);

        controllerBehaviour.weights = weights;

        return playable;
    }
    
    
}