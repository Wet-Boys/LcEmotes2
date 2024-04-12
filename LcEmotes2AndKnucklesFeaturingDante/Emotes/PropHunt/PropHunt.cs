using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Common;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.LightsCameraAction;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.Phone;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.PropHunt;
using LethalEmotesAPI.Core;
using LethalEmotesAPI.ImportV2;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Video;
using static UnityEngine.GraphicsBuffer;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.Megaman;

public class PropHunt : AbstractEmote
{
    public override string AnimationClipName => "move_into_floor_start";
    public override CustomEmoteParams GetClipParams()
    {
        return new CustomEmoteParams
        {
            primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/PropHunt/{AnimationClipName}.anim")],
            secondaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/PropHunt/move_into_floor_loop.anim")],
            primaryAudioClips = [Assets.Load<AudioClip>($"Emotes/PropHunt/houseLikeCarpet.ogg")],
            displayName = "Prop Mode",
            lockType = AnimationClipParams.LockType.none,
            thirdPerson = true,
            rootBonesToIgnore = [HumanBodyBones.RightUpperLeg, HumanBodyBones.LeftUpperLeg, HumanBodyBones.Spine]
        };
    }
    public override void SpawnProps(BoneMapper mapper)
    {
        GameObject g = Object.Instantiate(Assets.Load<GameObject>("Emotes/PropHunt/Cube.prefab"));
        mapper.props.Add(g);
        g.transform.SetParent(mapper.mapperBodyTransform);
        g.transform.localPosition = new Vector3(0,-1.33f,0);
        PropMover mover = g.AddComponent<PropMover>();
        mover.speed = 1;
        mover.delay = .33f;
    }
}