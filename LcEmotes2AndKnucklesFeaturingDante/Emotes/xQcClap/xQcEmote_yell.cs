using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Playables;
using UnityEngine;
using LethalEmotesAPI.ImportV2;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.xQcClap
{
    public class xQcEmote_yell : AbstractEmote
    {
        public override string AnimationClipName => "xQcYellAtSky";

        public override CustomEmoteParams GetClipParams()
        {
            return new CustomEmoteParams
            {
                primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/xQcClap/Animations/{AnimationClipName}.anim")],
                secondaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/xQcClap/xqc_clap_loop.anim")],
                primaryAudioClips = [Assets.Load<AudioClip>("Emotes/xQcClap/Audio/ahhhh2.ogg"), Assets.Load<AudioClip>("Emotes/xQcClap/Audio/ahhhh3.ogg"), Assets.Load<AudioClip>("Emotes/xQcClap/Audio/ahhhh4.ogg")],
                secondaryAudioClips = [Assets.Load<AudioClip>("Emotes/xQcClap/Audio/xqc_claploop.ogg"), Assets.Load<AudioClip>("Emotes/xQcClap/Audio/xqc_claploop.ogg"), Assets.Load<AudioClip>("Emotes/xQcClap/Audio/xqc_claploop.ogg")],
                audioLoops = false,
                syncAnim = false,
                syncAudio = false,
                displayName = "xQc Clap",
                visible = false,
                lockType = AnimationClipParams.LockType.headBobbing,
                willGetClaimedByDMCA = false,
                thirdPerson = false,
                rootBonesToIgnore = [HumanBodyBones.RightUpperLeg, HumanBodyBones.LeftUpperLeg],
                soloBonesToIgnore = [HumanBodyBones.Hips, HumanBodyBones.Spine, HumanBodyBones.Head],
                useLocalTransforms = true,
                audioLevel = 1
            };
        }

        public override void SpawnProps(BoneMapper mapper)
        {
            GameObject g = new GameObject();
            ClapHandler c = g.AddComponent<ClapHandler>();
            c.Setup(mapper, UnityEngine.Random.Range(.15f, .20f), "xqc_clap_loop");
            g.transform.SetParent(mapper.transform, false);
            mapper.props.Add(g);
        }
    }
}
