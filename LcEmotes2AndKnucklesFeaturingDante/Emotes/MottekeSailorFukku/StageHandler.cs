using EmotesAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Timeline;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.MottekeSailorFukku
{
    public class StageHandler : MonoBehaviour
    {
        private void Start()
        {
            MottekeSailorFukkuStage.currentStage = gameObject;
            MottekeSailorFukkuStage.dancers = [null, null, null, null, null, null, null, null, null, null];
            GameObject g2 = new GameObject();
            g2.transform.SetParent(gameObject.transform);
            new GameObject().transform.SetParent(g2.transform, false);
            DanceFreeCam test = g2.AddComponent<DanceFreeCam>();
            test.cameraObject = CustomEmotesAPI.localMapper.realCameraPos;
            MottekeSailorFukkuStage.currentCamera = g2;
            MottekeSailorFukkuStage.stageAudio = MottekeSailorFukkuStage.currentStage.GetComponent<AudioSource>();
            MottekeSailorFukkuStage.stageAudio.clip = Assets.Load<AudioClip>($"Emotes/Lucky Star/luckystar.ogg");
            StartCoroutine(FixScaleAfterTime());
        }
        IEnumerator FixScaleAfterTime()
        {
            yield return new WaitForEndOfFrame();
            for (int i = 1; i < 11; i++)
            {
                transform.GetChild(i).localScale = transform.localScale;
            }
        }
    }
}
