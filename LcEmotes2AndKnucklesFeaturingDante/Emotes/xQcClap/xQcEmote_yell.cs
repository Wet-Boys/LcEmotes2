using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Playables;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.xQcClap
{
    public class xQcEmote_yell : AbstractEmote
    {
        public override string AnimationClipName => "xQcYellAtSky";

        public override AnimationClipParams GetClipParams()
        {
            return new AnimationClipParams
            {
                animationClip = [Assets.Load<AnimationClip>($"Emotes/xQcClap/Animations/{AnimationClipName}.anim")],
                secondaryAnimation = [Assets.Load<AnimationClip>($"Emotes/xQcClap/xqc_clap_loop.anim")],
                _primaryAudioClips = [Assets.Load<AudioClip>("Emotes/xQcClap/Audio/ahhhh2.ogg"), Assets.Load<AudioClip>("Emotes/xQcClap/Audio/ahhhh3.ogg"), Assets.Load<AudioClip>("Emotes/xQcClap/Audio/ahhhh4.ogg")],
                _secondaryAudioClips = [Assets.Load<AudioClip>("Emotes/xQcClap/Audio/xqc_claploop.ogg"), Assets.Load<AudioClip>("Emotes/xQcClap/Audio/xqc_claploop.ogg"), Assets.Load<AudioClip>("Emotes/xQcClap/Audio/xqc_claploop.ogg")],
                looping = false,
                syncAnim = false,
                syncAudio = false,
                customName = "",
                visible = false,
                lockType = AnimationClipParams.LockType.headBobbing,
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
            c.Setup(mapper, UnityEngine.Random.Range(.15f, .20f), "xqc_clap_loop");
            g.transform.SetParent(mapper.transform, false);
            mapper.props.Add(g);
        }
    }
}
