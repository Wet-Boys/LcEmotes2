using EmotesAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Timeline;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.LightsCameraAction
{
    internal class UnFlattener : MonoBehaviour
    {
        public BoneMapper mapper;
        void OnDestroy()
        {
            mapper.basePlayerModelAnimator.gameObject.transform.localScale = new Vector3(mapper.basePlayerModelAnimator.gameObject.transform.localScale.x, mapper.basePlayerModelAnimator.gameObject.transform.localScale.x, mapper.basePlayerModelAnimator.gameObject.transform.localScale.z);
            if (mapper.local && CustomEmotesAPI.hudAnimator is not null)
            {
                CustomEmotesAPI.hudAnimator.transform.localScale = new Vector3(1, 1, 1);
            }
            mapper.transform.localScale = new Vector3(mapper.transform.localScale.x, mapper.transform.localScale.y, mapper.transform.localScale.x);
            if (CustomEmotesAPI.ModelReplacementAPIPresent && !mapper.isEnemy)
            {
                ModelReplacementAPICompat.ChangeModelScale(true, mapper.playerController);
            }
            if (CustomEmotesAPI.VRMPresent && !mapper.isEnemy)
            {
                LethalVRMCompat.ChangeModelScale(true, mapper.playerController);
            }
        }
    }
}
