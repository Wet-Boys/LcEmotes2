using EmotesAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.xQcClap
{
    public class ClapHandler : MonoBehaviour
    {
        public BoneMapper mapper;
        public float time = 0f;
        public string animation;
        void Start()
        {
            StartCoroutine(YellAfterTime());
        }
        IEnumerator YellAfterTime()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(time);
            if (CustomEmotesAPI.localMapper.isServer)
            {
                CustomEmotesAPI.PlayAnimation(animation, mapper);
            }
        }

        public void Setup(BoneMapper mapper, float time, string animation)
        {
            this.mapper = mapper;
            this.time = time;
            this.animation = animation;
        }
    }
}
