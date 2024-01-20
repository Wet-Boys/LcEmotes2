using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain;

public class GoblinPainEmote : AbstractEmote
{
    public override string AnimationClipName => "Goblin Pain Emote Scav";
    
    public override AnimationClipParams GetClipParams()
    {
        return new AnimationClipParams
        {
            animationClip = [Assets.Load<AnimationClip>($"Emotes/GoblinPain/{AnimationClipName}.anim")],
            _primaryAudioClips = [Assets.Load<AudioClip>("Emotes/GoblinPain/goblin-we-like-to-pain.ogg")],
            _primaryDMCAFreeAudioClips = [Assets.Load<AudioClip>("Emotes/GoblinPain/goblin-we-like-to-pain.ogg")],
            looping = true,
            syncAnim = true,
            syncAudio = true,
            customName = "Pain",
            lockType = AnimationClipParams.LockType.headBobbing,
            willGetClaimedByDMCA = false,
            thirdPerson = true
        };
    }

    public override void SpawnProps(BoneMapper mapper)
    {
        var propIndex = mapper.props.Count;
        mapper.props.Add(Object.Instantiate(Assets.Load<GameObject>("Emotes/GoblinPain/PainHolder.prefab")));
        mapper.props[propIndex].GetComponentInChildren<PlayableDirector>().time = CustomAnimationClip.syncTimer[mapper.currentClip.syncPos];
        mapper.props[propIndex].GetComponentInChildren<PlayableDirector>().Play();
        mapper.props[propIndex].GetComponentInChildren<TwitchChatController>().ownerPlayer = mapper.playerController;
        mapper.props[propIndex].GetComponentInChildren<TwitchChatDeath>().ownerPlayer = mapper.playerController;
        mapper.props[propIndex].transform.SetParent(mapper.mapperBody.transform);
        mapper.props[propIndex].transform.localEulerAngles = Vector3.zero;
        mapper.props[propIndex].transform.localPosition = new Vector3(0,1.0f,0);
        mappersPlayingPain.Add(mapper, mapper.props[propIndex].transform.Find("LyricsHolder"));
        foreach (var item in mappersPlayingPain)
        {
            item.Value.gameObject.SetActive(false);
        }
        if (mappersPlayingPain.ContainsKey(CustomEmotesAPI.localMapper))
        {
            mappersPlayingPain[CustomEmotesAPI.localMapper].gameObject.SetActive(true);
        }
        else
        {
            mappersPlayingPain.First().Value.gameObject.SetActive(true);
        }
    }
    internal static Dictionary<BoneMapper, Transform> mappersPlayingPain = new Dictionary<BoneMapper, Transform>();
}