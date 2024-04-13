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
    public class JermaWindowJump : AbstractEmote
    {
        public override string AnimationClipName => "Window1";
        public static List<int> props = new List<int>();
        public override CustomEmoteParams GetClipParams()
        {
            return new CustomEmoteParams
            {
                primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/JermaWindow/{AnimationClipName}.anim")],
                primaryAudioClips = [Assets.Load<AudioClip>("Emotes/JermaWindow/Shatter1.ogg"), Assets.Load<AudioClip>("Emotes/JermaWindow/Shatter2.ogg"), Assets.Load<AudioClip>("Emotes/JermaWindow/Shatter3.ogg"), Assets.Load<AudioClip>("Emotes/JermaWindow/Shatter4.ogg"), Assets.Load<AudioClip>("Emotes/JermaWindow/Shatter5.ogg")],
                primaryDMCAFreeAudioClips = [Assets.Load<AudioClip>("Emotes/JermaWindow/Shatter1.ogg"), Assets.Load<AudioClip>("Emotes/JermaWindow/Shatter2.ogg"), Assets.Load<AudioClip>("Emotes/JermaWindow/Shatter3.ogg"), Assets.Load<AudioClip>("Emotes/JermaWindow/Shatter4.ogg"), Assets.Load<AudioClip>("Emotes/JermaWindow/Shatter5.ogg")],
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
            mapper.StartCoroutine(AddFallCheckerAfterTime(mapper));
        }
        IEnumerator AddFallCheckerAfterTime(BoneMapper mapper)
        {
            yield return new WaitForSeconds(.75f);
            GameObject g = new GameObject();
            WindowFallChecker fallChecker = g.AddComponent<WindowFallChecker>();
            fallChecker.bodyPosition = mapper.mapperBodyTransform;
            fallChecker.mapper = mapper;
            mapper.props.Add(g);
        }
    }
}
