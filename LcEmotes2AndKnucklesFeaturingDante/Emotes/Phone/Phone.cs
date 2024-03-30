using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Common;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.LightsCameraAction;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.Phone;
using LethalEmotesAPI.Core;
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
    internal static List<VideoClip> videoClips = [Assets.Load<VideoClip>($"Emotes/BoringAhhGame/SubwaySurfers.mp4"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/MinecraftParkour.webm"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/FamilyGuy.mp4"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/Satisfying.mp4"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/SliceIt.webm")];
    internal static List<VideoClip> easterEggVideoClips = [Assets.Load<VideoClip>($"Emotes/BoringAhhGame/BadShrekle.webm"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/GreyLeno.webm"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/KoopaCape.mp4"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/Lifesaver.mp4"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/Megaman.mp4"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/True.mp4"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/oldspice.webm"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/octagon.webm"), Assets.Load<VideoClip>($"Emotes/BoringAhhGame/kitchengun.webm")];
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
        Transform target = mapper.mapperBody.transform.Find($"ScavengerModel/metarig/spine/spine.001/spine.002/spine.003/shoulder.L/arm.L_upper/arm.L_lower/hand.L");
        if (target is null)
        {
            BoneRef boneRef = mapper.emoteSkeletonAnimator.GetBoneTransform(HumanBodyBones.LeftHand).GetComponent<BoneRef>();
            if (boneRef is not null)
            {
                prop.transform.parent = boneRef.target;
            }
        }
        else
        {
            DebugClass.Log($"got a target");
            prop.transform.parent = target;
        }
        prop.transform.localEulerAngles = new Vector3(1, 90, 255);
        prop.transform.localPosition = new Vector3(-0.0127f, 0.0964f, -0.0436f);
        prop.transform.localScale = new Vector3(0.065f, 0.065f, 0.065f);
        prop.GetComponentInChildren<VideoPlayer>().SetTargetAudioSource(0, mapper.personalAudioSource);
        mapper.StartCoroutine(stopDoingThat(mapper));

        if (CustomEmotesAPI.localMapper.isServer)
        {
            Emotes2Networking.instance.SyncRandomVideoToClientRpc(mapper.mapperBody.GetComponent<NetworkObject>().NetworkObjectId, UnityEngine.Random.Range(0, videoClips.Count), UnityEngine.Random.Range(0, easterEggVideoClips.Count), UnityEngine.Random.Range(0, 100), UnityEngine.Random.Range(0f, 1f));
        }
        //look into this later?
        //if (mapper.local && mapper.isValidPlayer)
        //{
        //    Transform shoulder = mapper.playerController.thisPlayerModelArms.bones[1];
        //    if (shoulder.name != "shoulder.R") //WHY >:L
        //    {
        //        foreach (var item in mapper.playerController.thisPlayerModelArms.bones)
        //        {
        //            if (item.name == "shoulder.R")
        //            {
        //                shoulder = item;
        //                break;
        //            }
        //        }
        //    }
        //    prop.AddComponent<ArmFixer>().Setup(mapper.emoteSkeletonAnimator.GetBoneTransform(HumanBodyBones.LeftShoulder).GetComponent<BoneRef>().target, shoulder);
        //}
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