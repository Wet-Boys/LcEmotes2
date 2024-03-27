using System.Collections.Generic;
using System.Linq;
using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.LightsCameraAction;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.Megaman;
using LcEmotes2AndKnucklesFeaturingDante.JoinSpots;
using LethalEmotesAPI.ImportV2;
using UnityEngine;
using UnityEngine.Timeline;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes;

internal static class EmoteRegistry
{
    private static bool _finished;
    private static readonly Dictionary<string, AbstractEmote> Emotes = [];
    private static readonly Dictionary<string, AbstractJoinSpot> JoinSpots = [];


    public static void RegisterEmote<T>()
        where T : AbstractEmote, new()
    {
        var emote = new T();
        Emotes[$"{LcEmotes2AndKnucklesFeaturingDantePlugin.ModGuid}__{emote.AnimationClipName}"] = emote;
    }

    public static void RegisterEmote<T>(T emoteInstance)
        where T : AbstractEmote
    {
        Emotes[$"{LcEmotes2AndKnucklesFeaturingDantePlugin.ModGuid}__{emoteInstance.AnimationClipName}"] = emoteInstance;
    }

    public static void RegiserProp(List<int> props, GameObject prop, JoinSpot[] joinSpots, AbstractJoinSpot[] joinSpotClasses)
    {
        props.Add(CustomEmotesAPI.RegisterWorldProp(prop, joinSpots));
        for (int i = 0; i < joinSpots.Length; i++)
        {
            JoinSpots[joinSpots[i].name] = joinSpotClasses[i];
        }
    }

    public static void FinalizeRegistry()
    {
        if (_finished)
            return;

        foreach (var (_, emote) in Emotes)
            EmoteImporter.ImportEmote(emote.GetClipParams());

        CustomEmotesAPI.animChanged += OnSpawnWorldProps;
        CustomEmotesAPI.emoteSpotJoined_Prop += OnJoinedWorldProp;
        _finished = true;
    }

    private static void OnJoinedWorldProp(GameObject emoteSpot, BoneMapper joiner, BoneMapper host)
    {
        if (!JoinSpots.TryGetValue(emoteSpot.name, out var emote))
            return;

        emote.OnSpotJoined(emoteSpot, joiner, host);
    }

    private static void OnSpawnWorldProps(string animName, BoneMapper mapper)
    {
        GoblinPainEmote.mappersPlayingPain.Remove(mapper);
        if (GoblinPainEmote.mappersPlayingPain.ContainsKey(CustomEmotesAPI.localMapper))
        {
            GoblinPainEmote.mappersPlayingPain[CustomEmotesAPI.localMapper].gameObject.SetActive(true);
        }
        else if (GoblinPainEmote.mappersPlayingPain.Count != 0)
        {
            GoblinPainEmote.mappersPlayingPain.First().Value.gameObject.SetActive(true);
        }
        if (LightsCameraActionEmote.flatMappers.Contains(mapper))
        {
            LightsCameraActionEmote.flatMappers.Remove(mapper);
            mapper.basePlayerModelAnimator.gameObject.transform.localScale = new Vector3(mapper.basePlayerModelAnimator.gameObject.transform.localScale.x, mapper.basePlayerModelAnimator.gameObject.transform.localScale.x, mapper.basePlayerModelAnimator.gameObject.transform.localScale.z);
            if (mapper.local)
            {
                CustomEmotesAPI.hudAnimator.transform.localScale = new Vector3(1, 1, 1);
                mapper.transform.localScale = new Vector3(mapper.transform.localScale.x, mapper.transform.localScale.y, mapper.transform.localScale.x);
            }
            if (CustomEmotesAPI.ModelReplacementAPIPresent && !mapper.isEnemy)
            {
                ModelReplacementAPICompat.ChangeModelScale(true, mapper.playerController);
            }
            if (CustomEmotesAPI.VRMPresent && !mapper.isEnemy)
            {
                LethalVRMCompat.ChangeModelScale(true, mapper.playerController);
            }
        }


        if (!Emotes.TryGetValue(animName, out var emote))
            return;

        emote.SpawnProps(mapper);
    }
}