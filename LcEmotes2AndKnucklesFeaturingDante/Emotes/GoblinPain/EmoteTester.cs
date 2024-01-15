using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain
{
    internal class EmoteTester : MonoBehaviour
    {
        public List<Animator> animators = new List<Animator>();
        public AudioSource audioSource;
        void Start()
        {
            foreach (var item in animators)
            {
                item.StopPlayback();
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                foreach (var item in animators)
                {
                    item.Play(item.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, 0);
                }
                audioSource.Stop();
                audioSource.Play();
            }
            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    foreach (var item in animators)
            //    {
            //        item.StopPlayback();
            //    }
            //    audioSource.Stop();
            //}
        }
    }
}
