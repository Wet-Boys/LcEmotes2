using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Playables;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.xQcClap
{
    public class xQcEmote_clap : AbstractEmote
    {
        public override string AnimationClipName => "xqc_clap_loop";

        public override AnimationClipParams GetClipParams()
        {
            return new AnimationClipParams
            {
                animationClip = [Assets.Load<AnimationClip>($"Emotes/xQcClap/xqc_clap_loop.anim")],
                _primaryAudioClips = [Assets.Load<AudioClip>("Emotes/xQcClap/Audio/xqc_claploop.ogg")],
                looping = true,
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
            float time;
            if (UnityEngine.Random.Range(0, 10) < 4)
            {
                time = UnityEngine.Random.Range(.075f, .20f);
            }
            else
            {
                time = UnityEngine.Random.Range(3f, 7f);

            }
            c.Setup(mapper, time, "xQcYellAtSky");
            g.transform.SetParent(mapper.transform, false);
            mapper.props.Add(g);
        }
    }
}
