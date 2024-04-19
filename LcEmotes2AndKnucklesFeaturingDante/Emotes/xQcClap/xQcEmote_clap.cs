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
    public class xQcEmote_clap : AbstractEmote
    {
        public override string AnimationClipName => "xqc_clap_loop";

        public override CustomEmoteParams GetClipParams()
        {
            return new CustomEmoteParams
            {
                primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/xQcClap/xqc_clap_loop.anim")],
                primaryAudioClips = [Assets.Load<AudioClip>("Emotes/xQcClap/Audio/xqc_claploop.ogg")],
                audioLoops = true,
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
                audioLevel = 1,
                emoteToPlayOnJoin = $"{LcEmotes2AndKnucklesFeaturingDantePlugin.ModGuid}__xqc_clap_start"
            };
        }

        public override void SpawnProps(BoneMapper mapper)
        {
            if (mapper.prevClipName != $"{LcEmotes2AndKnucklesFeaturingDantePlugin.ModGuid}__xQcYellAtSky")
            {
                if (CustomEmotesAPI.localMapper.isServer)
                {
                    CustomEmotesAPI.PlayAnimation($"{LcEmotes2AndKnucklesFeaturingDantePlugin.ModGuid}__xqc_clap_start", mapper);
                }
                return;
            }
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

            c.SpawnFX(mapper);
        }
    }
}
