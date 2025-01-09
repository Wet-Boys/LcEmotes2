using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Timeline;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.MottekeSailorFukku
{
    public class MottekeSailorFukkuCleanup : MonoBehaviour
    {
        public int spotInArray = -1;
        public BoneMapper mapper;
        public GameObject emoteSpot;
        void OnDestroy()
        {
            MottekeSailorFukkuStage.dancers[spotInArray] = null;
            if (mapper.local)
            {
                HUDManager.Instance.SetNearDepthOfFieldEnabled(true);
                DanceFreeCam.instance.active = false;
                EmoteLocation yote = emoteSpot.GetComponent<EmoteLocation>();
                try
                {
                    foreach (var item2 in yote.GetComponentsInChildren<Renderer>())
                    {
                        item2.enabled = true;
                    }
                    foreach (var item2 in yote.GetComponentsInChildren<ParticleSystemRenderer>())
                    {
                        item2.enabled = true;
                    }
                }
                catch (Exception)
                {
                }
                MottekeSailorFukkuStage.stageAudio.spatialBlend = 1;
            }
            if (emoteSpot.transform.localPosition.x > 50)
            {
                emoteSpot.transform.localPosition -= new Vector3(5000, 5000, 5000);
            }
            foreach (var item in MottekeSailorFukkuStage.dancers)
            {
                if (item is not null)
                {
                    //DebugClass.Log($"{item} was not null so we aren't stopping music");
                    return;
                }
            }
            MottekeSailorFukkuStage.stageAudio.Stop();
        }
    }
}
