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

        internal IEnumerator SpawnClapFXAfterTime(float time, BoneMapper mapper)
        {
            yield return new WaitForSeconds(time);
            SpawnFX(mapper);
        }
        internal void SpawnFX(BoneMapper mapper)
        {
            var propIndex = mapper.props.Count;
            mapper.props.Add(UnityEngine.Object.Instantiate(Assets.Load<GameObject>("Emotes/xQcClap/VFX/ClapHandFX2.prefab")));
            mapper.props[propIndex].transform.SetParent(mapper.basePlayerModelSMR[0].bones[3]);
            mapper.props[propIndex].transform.localPosition = new Vector3(-.02f, -0.125f, .58f);


            propIndex = mapper.props.Count;
            mapper.props.Add(UnityEngine.Object.Instantiate(Assets.Load<GameObject>("Emotes/xQcClap/VFX/WorbleCylinder.prefab")));
            mapper.props[propIndex].transform.SetParent(mapper.basePlayerModelSMR[0].bones[3]);
            mapper.props[propIndex].transform.localPosition = new Vector3(-.02f, -0.125f, .58f);
        }

        public void Setup(BoneMapper mapper, float time, string animation)
        {
            this.mapper = mapper;
            this.time = time;
            this.animation = animation;
        }
    }
}
