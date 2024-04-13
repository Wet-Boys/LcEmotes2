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
using UnityEngine.UIElements;
using Unity.Netcode;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.JermaWindow
{
    public class JermaWindowFall : AbstractEmote
    {
        public override string AnimationClipName => "falling";
        public static List<int> props = new List<int>();
        public override CustomEmoteParams GetClipParams()
        {
            return new CustomEmoteParams
            {
                primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/JermaWindow/{AnimationClipName}.anim")],
                displayName = "Jerma Window",
                lockType = AnimationClipParams.LockType.rootMotion,
                willGetClaimedByDMCA = false,
                preventMovement = true,
                allowJoining = false,
                visible = false
            };
        }

        public override void SpawnProps(BoneMapper mapper)
        {
        }
    }
}
