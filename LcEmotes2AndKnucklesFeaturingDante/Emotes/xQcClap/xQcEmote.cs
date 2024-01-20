using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Playables;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.xQcClap
{
    public class xQcEmote : AbstractEmote
    {
        public override string AnimationClipName => "xqc_clap_start";

        public override AnimationClipParams GetClipParams()
        {
            return new AnimationClipParams
            {
                animationClip = [Assets.Load<AnimationClip>($"Emotes/xQcClap/{AnimationClipName}.anim")],
                secondaryAnimation = [Assets.Load<AnimationClip>($"Emotes/xQcClap/xqc_clap_loop.anim")],
                _primaryAudioClips = [Assets.Load<AudioClip>("Emotes/xQcClap/Audio/xqc_wow.ogg")],
                _secondaryAudioClips = [Assets.Load<AudioClip>("Emotes/xQcClap/Audio/xqc_claploop.ogg")],
                looping = true,
                syncAnim = false,
                syncAudio = false,
                customName = "xQc Clap",
                lockType = AnimationClipParams.LockType.lockHead,
                willGetClaimedByDMCA = false,
                thirdPerson = false,
                rootBonesToIgnore = [HumanBodyBones.RightUpperLeg, HumanBodyBones.LeftUpperLeg],
                soloBonesToIgnore = [HumanBodyBones.Hips, HumanBodyBones.Spine],
                useLocalTransforms = true
            };
        }

        public override void SpawnProps(BoneMapper mapper)
        {

        }
    }
}
