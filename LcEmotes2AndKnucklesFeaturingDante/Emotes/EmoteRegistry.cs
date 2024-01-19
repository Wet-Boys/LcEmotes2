using System.Collections.Generic;
using EmotesAPI;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes;

internal static class EmoteRegistry
{
    private static bool _finished;
    private static readonly Dictionary<string, AbstractEmote> Emotes = [];

    public static void RegisterEmote<T>()
        where T : AbstractEmote, new()
    {
        var emote = new T();
        Emotes[emote.AnimationClipName] = emote;
    }

    public static void RegisterEmote<T>(T emoteInstance)
        where T : AbstractEmote
    {
        Emotes[emoteInstance.AnimationClipName] = emoteInstance;
    }

    public static void FinalizeRegistry()
    {
        if (_finished)
            return;

        foreach (var (_, emote) in Emotes)
            CustomEmotesAPI.AddCustomAnimation(emote.GetClipParams());
        
        CustomEmotesAPI.animChanged += OnSpawnWorldProps;
        _finished = true;
    }

    private static void OnSpawnWorldProps(string animName, BoneMapper mapper)
    {
        if (!Emotes.TryGetValue(animName, out var emote))
            return;
        
        emote.SpawnProps(mapper);
    }
}