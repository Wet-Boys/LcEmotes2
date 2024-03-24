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
    public class TestEmote : AbstractEmote
    {
        public override string AnimationClipName => "avatartesting";

        public override CustomEmoteParams GetClipParams()
        {
            return new CustomEmoteParams
            {
                primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/xQcClap/Testing/{AnimationClipName}.anim")],
                syncAnim = false,
                syncAudio = false,
                displayName = "Finger Touch",
                lockType = AnimationClipParams.LockType.none,
                willGetClaimedByDMCA = false,
                thirdPerson = true,
            };
        }

        public override void SpawnProps(BoneMapper mapper)
        {
        }
    }
}
