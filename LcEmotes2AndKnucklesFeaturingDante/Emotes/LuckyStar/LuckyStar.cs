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
    public class LuckyStar : AbstractEmote
    {
        public override string AnimationClipName => "testDance";

        public override CustomEmoteParams GetClipParams()
        {
            return new CustomEmoteParams
            {
                primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/Lucky Star/{AnimationClipName}.anim")],
                primaryAudioClips = [Assets.Load<AudioClip>("Emotes/Lucky Star/luckystar.ogg")],
                audioLoops = false,
                syncAnim = false,
                syncAudio = false,
                displayName = "Motteke! Sailor Fuku",
                lockType = AnimationClipParams.LockType.rootMotion,
                willGetClaimedByDMCA = true,
                thirdPerson = true,
                audioLevel = 1
            };
        }

        public override void SpawnProps(BoneMapper mapper)
        {

        }
    }
}
