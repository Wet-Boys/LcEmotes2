using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.LightsCameraAction;
using LethalEmotesAPI.ImportV2;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.Megaman;

public class PhoneEmote : AbstractEmote
{
    public override string AnimationClipName => "phone_start";
    internal static List<BoneMapper> flatMappers = new List<BoneMapper>();
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

    }
}