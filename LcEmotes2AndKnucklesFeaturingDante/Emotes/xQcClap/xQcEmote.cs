using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Playables;
using UnityEngine;
using System.Collections;
using UnityEngine.Timeline;
using LethalEmotesAPI.ImportV2;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.xQcClap
{
    public class xQcEmote : AbstractEmote
    {
        public override string AnimationClipName => "xqc_clap_start";

        public override CustomEmoteParams GetClipParams()
        {
            return new CustomEmoteParams
            {
                primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/xQcClap/{AnimationClipName}.anim")],
                secondaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/xQcClap/xqc_clap_loop.anim")],
                primaryAudioClips = [Assets.Load<AudioClip>("Emotes/xQcClap/Audio/xqc_wow.ogg")],
                secondaryAudioClips = [Assets.Load<AudioClip>("Emotes/xQcClap/Audio/xqc_claploop.ogg")],
                audioLoops = true,
                syncAnim = false,
                syncAudio = false,
                displayName = "xQc Clap",
                lockType = AnimationClipParams.LockType.lockHead,
                willGetClaimedByDMCA = false,
                thirdPerson = false,
                rootBonesToIgnore = [HumanBodyBones.RightUpperLeg, HumanBodyBones.LeftUpperLeg],
                soloBonesToIgnore = [HumanBodyBones.Hips, HumanBodyBones.Spine],
                useLocalTransforms = true,
                audioLevel = 1
            };
        }

        public override void SpawnProps(BoneMapper mapper)
        {
            GameObject g = new GameObject();
            ClapHandler c = g.AddComponent<ClapHandler>();
            c.Setup(mapper, UnityEngine.Random.Range(3f,7f), "xQcYellAtSky");
            g.transform.SetParent(mapper.transform, false);
            mapper.props.Add(g);
            c.StartCoroutine(c.SpawnClapFXAfterTime(2f, mapper));
        }
    }
}
