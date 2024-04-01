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

public class LightsCameraActionEmote : AbstractEmote
{
    public override string AnimationClipName => "LightsCameraActionStart";
    internal static List<BoneMapper> flatMappers = new List<BoneMapper>();
    public override CustomEmoteParams GetClipParams()
    {
        return new CustomEmoteParams
        {
            primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/LightsCameraAction/{AnimationClipName}.anim")],
            secondaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/LightsCameraAction/LightsCameraActionLoop.anim")],
            primaryAudioClips = [Assets.Load<AudioClip>("Emotes/LightsCameraAction/LightsCameraActionStart.ogg")],
            secondaryAudioClips = [Assets.Load<AudioClip>("Emotes/LightsCameraAction/LightsCameraActionLoop.ogg")],
            primaryDMCAFreeAudioClips = [Assets.Load<AudioClip>("Emotes/LightsCameraAction/LightsCameraActionStart.ogg")],
            secondaryDMCAFreeAudioClips = [Assets.Load<AudioClip>("Emotes/LightsCameraAction/LightsCameraActionLoop.ogg")],
            audioLoops = true,
            syncAnim = true,
            syncAudio = true,
            displayName = "Lights Camera Action",
            lockType = AnimationClipParams.LockType.headBobbing,
            willGetClaimedByDMCA = false,
            thirdPerson = true
        };
    }
    internal static IEnumerator FinishPropAfterFrame(BoneMapper mapper)
    {
        yield return new WaitForEndOfFrame();
        mapper.basePlayerModelAnimator.gameObject.transform.localScale = new Vector3(mapper.basePlayerModelAnimator.gameObject.transform.localScale.x, .001f, mapper.basePlayerModelAnimator.gameObject.transform.localScale.z);
        if (!mapper.isEnemy)
        {
            if (CustomEmotesAPI.ModelReplacementAPIPresent)
            {
                ModelReplacementAPICompat.ChangeModelScale(false, mapper.playerController);
            }
            if (CustomEmotesAPI.VRMPresent)
            {
                LethalVRMCompat.ChangeModelScale(false, mapper.playerController);
            }
        }
    }
    public override void SpawnProps(BoneMapper mapper)
    {
        if (!flatMappers.Contains(mapper))
        {
            flatMappers.Add(mapper);
            if (mapper.local && CustomEmotesAPI.hudAnimator is not null)
            {
                CustomEmotesAPI.hudAnimator.transform.localScale = new Vector3(1, .001f, 1);
            }
            mapper.transform.localScale = new Vector3(mapper.transform.localScale.x, mapper.transform.localScale.y, mapper.transform.localScale.x * .001f);
        }
        mapper.StartCoroutine(FinishPropAfterFrame(mapper));
        if (!(LcEmotes2AndKnucklesFeaturingDantePlugin.watermarkRemoverPresent && WatermarkCompaty.CantHaveWatermark()))
        {
            var propIndex = mapper.props.Count;
            mapper.props.Add(Object.Instantiate(Assets.Load<GameObject>("Emotes/LightsCameraAction/Watermark.prefab")));
            mapper.props[propIndex].transform.localScale = new Vector3(.6f, .6f, .6f);
            WatermarkComponent rot = mapper.props[propIndex].AddComponent<WatermarkComponent>();
            rot.target = mapper.mapperBody.transform;
            rot.trackedBone = mapper.emoteSkeletonAnimator.GetBoneTransform(HumanBodyBones.Spine);
            rot.multiplier = .001f;
            rot.player = mapper.playerController;

            propIndex = mapper.props.Count;
            mapper.props.Add(Object.Instantiate(Assets.Load<GameObject>("Emotes/LightsCameraAction/Watermark.prefab")));
            mapper.props[propIndex].transform.localScale = new Vector3(.6f, .6f, .6f);
            rot = mapper.props[propIndex].AddComponent<WatermarkComponent>();
            rot.target = mapper.mapperBody.transform;
            rot.trackedBone = mapper.emoteSkeletonAnimator.GetBoneTransform(HumanBodyBones.Spine);
            rot.multiplier = -.005f;
            rot.player = mapper.playerController;
        }
    }
}