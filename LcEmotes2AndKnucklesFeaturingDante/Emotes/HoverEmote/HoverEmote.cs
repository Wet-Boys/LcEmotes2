using EmotesAPI;
using LethalEmotesAPI.ImportV2;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.HoverEmote
{
    public class HoverEmote : AbstractEmote
    {
        public override string AnimationClipName => "HoverStart";

        public override CustomEmoteParams GetClipParams()
        {
            return new CustomEmoteParams
            {
                primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/Hover/{AnimationClipName}.anim")],
                secondaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/Hover/HoverLoop.anim")],
                displayName = "Hover",
                lockType = AnimationClipParams.LockType.headBobbing,
                audioLevel = 0,
            };
        }

        public override void SpawnProps(BoneMapper mapper)
        {
            var propIndex = mapper.props.Count;
            mapper.props.Add(Object.Instantiate(Assets.Load<GameObject>("Emotes/Hover/HoverLoop1Ball.prefab")));
            mapper.props[propIndex].transform.SetParent(mapper.mapperBody.transform);
            mapper.props[propIndex].transform.localEulerAngles = Vector3.zero;
            mapper.props[propIndex].transform.localPosition = new Vector3(0, 0, 0);
            mapper.AttachItemHolderToTransform(mapper.props[propIndex].transform);
        }
    }
}
