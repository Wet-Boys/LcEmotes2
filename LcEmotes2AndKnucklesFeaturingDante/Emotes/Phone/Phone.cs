using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Common;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.LightsCameraAction;
using LethalEmotesAPI.ImportV2;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Video;
using static UnityEngine.GraphicsBuffer;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.Megaman;

public class PhoneEmote : AbstractEmote
{
    public override string AnimationClipName => "phone_start";
    internal static List<BoneMapper> flatMappers = new List<BoneMapper>();
    internal static List<VideoClip> videoClips = [Assets.Load<VideoClip>($"Emotes/BoringAhhGame/SubwaySurfers.mp4"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/MinecraftParkour.mp4"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/FamilyGuy.mp4"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/Satisfying.mp4")];
    internal static List<VideoClip> easterEggVideoClips = [Assets.Load<VideoClip>($"Emotes/BoringAhhGame/BadShrekle.mp4"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/GreyLeno.mp4")];
    public override CustomEmoteParams GetClipParams()
    {
        return new CustomEmoteParams
        {
            primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/BoringAhhGame/{AnimationClipName}.anim")],
            secondaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/BoringAhhGame/phone_loop.anim")],
            displayName = "Boring Ahh Game",
            lockType = AnimationClipParams.LockType.headBobbing,
            useLocalTransforms = true,
            rootBonesToIgnore = [HumanBodyBones.RightShoulder, HumanBodyBones.LeftUpperLeg, HumanBodyBones.RightUpperLeg, HumanBodyBones.Head],
            soloBonesToIgnore = [HumanBodyBones.Spine, HumanBodyBones.Chest, HumanBodyBones.Hips, HumanBodyBones.UpperChest]
        };
    }
    public override void SpawnProps(BoneMapper mapper)
    {
        var propIndex = mapper.props.Count;
        mapper.props.Add(Object.Instantiate(Assets.Load<GameObject>("Emotes/BoringAhhGame/iphone4.prefab")));
        GameObject prop = mapper.props[propIndex];
        foreach (var item in mapper.basePlayerModelSMR[0].bones)
        {
            if (item.GetComponent<EmoteConstraint>() is not null && item.GetComponent<EmoteConstraint>().emoteBone == mapper.emoteSkeletonAnimator.GetBoneTransform(HumanBodyBones.LeftHand))
            {
                prop.transform.parent = item;
            }
        }
        prop.transform.localEulerAngles = new Vector3(1, 90, 255);
        prop.transform.localPosition = new Vector3(-0.0127f, 0.0964f, -0.0436f);
        prop.transform.localScale = new Vector3(0.065f, 0.065f, 0.065f);
        prop.GetComponentInChildren<VideoPlayer>().SetTargetAudioSource(0, mapper.personalAudioSource);
        mapper.StartCoroutine(stopDoingThat(mapper));

        if (CustomEmotesAPI.localMapper.isServer)
        {
            Emotes2Networking.instance.SyncRandomVideoToClientRpc(mapper.mapperBody.GetComponent<NetworkObject>().NetworkObjectId, UnityEngine.Random.Range(0,videoClips.Count), UnityEngine.Random.Range(0, easterEggVideoClips.Count), UnityEngine.Random.Range(0, 100));
        }
        //network to clients if server
    }
    IEnumerator stopDoingThat(BoneMapper mapper)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        foreach (var item in mapper.itemHolderConstraints)
        {
            item.DeactivateConstraints();
        }
    }
}