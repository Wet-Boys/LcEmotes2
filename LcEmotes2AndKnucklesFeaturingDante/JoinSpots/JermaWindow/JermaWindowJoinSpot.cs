using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.JermaWindow;
using LethalEmotesAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.JoinSpots.JermaWindow
{
    public class JermaWindowJoinSpot : AbstractJoinSpot
    {
        public override void OnSpotJoined(GameObject emoteSpot, BoneMapper joiner, BoneMapper host)
        {
            joiner.mapperBodyTransform.position = emoteSpot.transform.position;
            joiner.mapperBodyTransform.rotation = emoteSpot.transform.rotation;
            joiner.mapperBodyTransform.localEulerAngles += new Vector3(0, 180, 0);
            joiner.PlayAnim($"{LcEmotes2AndKnucklesFeaturingDantePlugin.ModGuid}__Window1", -1);
            if (joiner.playerController is not null && joiner.local)
            {
                GameObject g = new GameObject();
                joiner.props.Add(g);
                g.AddComponent<CameraLocker>().cam = joiner.playerController.gameplayCamera;
            }
            joiner.StartCoroutine(SpawnBrokenGlass(.6166f, host.transform, joiner));

            EmoteLocation emoteLocation = emoteSpot.GetComponent<EmoteLocation>();
            emoteLocation.SetEmoterAndHideLocation(joiner);
            emoteLocation.gameObject.transform.localPosition -= new Vector3(5000, 5000, 5000);
            host.GetComponent<WindowHandler>().owner = null;
            if (!joiner.currentClipName.Equals($"{LcEmotes2AndKnucklesFeaturingDantePlugin.ModGuid}__Window1"))
            {
                AudioSource windowAudioSource = host.transform.Find("Base").GetComponent<AudioSource>();
                windowAudioSource.clip = joiner.personalAudioSource.clip;
                joiner.personalAudioSource.GetComponent<AudioManager>().Stop();
                windowAudioSource.Play();
            }
        }
        internal static IEnumerator SpawnBrokenGlass(float delay, Transform hostPosition, BoneMapper owner)
        {
            yield return new WaitForSeconds(delay);
            if (owner is not null && owner.currentClipName.Equals($"{LcEmotes2AndKnucklesFeaturingDantePlugin.ModGuid}__Window1"))
            {
                GameObject g = UnityEngine.Object.Instantiate(Assets.Load<GameObject>("Emotes/JermaWindow/WindowShatter.prefab"));
                g.transform.SetParent(hostPosition);
                g.transform.localPosition = Vector3.zero;
                g.transform.localRotation = Quaternion.identity;
                g.transform.localScale = Vector3.one;
                hostPosition.Find("Glass").gameObject.SetActive(false);
                hostPosition.Find("XBeam").gameObject.SetActive(false);
                hostPosition.Find("ZBeam").gameObject.SetActive(false);
            }
            GameObject g2 = UnityEngine.Object.Instantiate(Assets.Load<GameObject>("Emotes/JermaWindow/Explosion.prefab"));
            g2.transform.SetParent(hostPosition);
            g2.transform.localPosition = new Vector3(0, 1.33f, 0);
            g2.transform.localScale = new Vector3(5, 5, 5);
            g2.transform.SetParent(g2.transform.parent.parent);
            yield return new WaitForSeconds(5 - delay);
            CustomEmotesAPI.localMapper.StartCoroutine(CleanupExplosion(g2));
            g2.GetComponent<ParticleSystem>().Emit(1);
            g2.GetComponent<AudioSource>().Play();
            if (CustomEmotesAPI.localMapper.isServer)
            {
                GameObject.Destroy(hostPosition.gameObject);
            }
        }
        internal static IEnumerator CleanupExplosion(GameObject g)
        {
            yield return new WaitForSeconds(3);
            UnityEngine.Object.Destroy(g);
        }
    }
}
