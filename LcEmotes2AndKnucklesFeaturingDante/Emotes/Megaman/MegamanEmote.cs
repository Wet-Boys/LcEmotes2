using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using LethalEmotesAPI.ImportV2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.Megaman;

public class MegamanEmote : AbstractEmote
{
    public override string AnimationClipName => "Window1";
    
    public override CustomEmoteParams GetClipParams()
    {
        return new CustomEmoteParams
        {
            primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/JermaWindow/{AnimationClipName}.anim")],
            primaryAudioClips = [Assets.Load<AudioClip>("Emotes/Megaman/Megaman-mariogalaxy.ogg")],
            //primaryDMCAFreeAudioClips = [Assets.Load<AudioClip>("Emotes/GoblinPain/goblin-we-like-to-pain.ogg")],
            audioLoops = true,
            syncAnim = true,
            syncAudio = true,
            displayName = "Money Smart",
            lockType = AnimationClipParams.LockType.rootMotion,
            willGetClaimedByDMCA = false,
            thirdPerson = false
        };
    }

    public override void SpawnProps(BoneMapper mapper)
    {
        //mapper.SetAnimationSpeed(.833f);
        //mapper.SetAutoWalk(1f, false);
    }
}