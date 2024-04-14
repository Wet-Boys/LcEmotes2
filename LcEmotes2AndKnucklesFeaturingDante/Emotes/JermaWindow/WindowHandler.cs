using EmotesAPI;
using GameNetcodeStuff;
using LcEmotes2AndKnucklesFeaturingDante.Common;
using LcEmotes2AndKnucklesFeaturingDante.JoinSpots.JermaWindow;
using LethalEmotesAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.JermaWindow
{
    public class WindowHandler : MonoBehaviour, IHittable
    {
        public static List<WindowHandler> allInactiveWindows = new List<WindowHandler>();
        public BoneMapper owner;
        public bool destroying = false;
        void Start()
        {
            allInactiveWindows.Add(this);
        }
        void OnDestroy()
        {
            allInactiveWindows.Remove(this);
        }
        void Update()
        {
            if (destroying)
            {
                owner = null;
            }
        }

        public bool Hit(int force, Vector3 hitDirection, PlayerControllerB playerWhoHit = null, bool playHitSFX = false)
        {
            OnHit();
            return true;
        }
        public bool Hit(int force, Vector3 hitDirection, PlayerControllerB playerWhoHit = null, bool playHitSFX = false, int hitID = -1)
        {
            OnHit();
            return true;
        }
        public void OnHit()
        {
            if (!destroying)
            {
                destroying = true;
                Emotes2Networking.instance.SyncGlassBreak(GetComponent<NetworkObject>().NetworkObjectId);
            }
        }
        public void BreakGlass()
        {
            EmoteLocation emoteLocation = GetComponentInChildren<EmoteLocation>();
            emoteLocation.gameObject.transform.localPosition -= new Vector3(5000, 5000, 5000);
            owner = null;
            GameObject g = UnityEngine.Object.Instantiate(Assets.Load<GameObject>("Emotes/JermaWindow/WindowShatter.prefab"));
            g.transform.SetParent(transform);
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            g.transform.localScale = Vector3.one;
            transform.Find("Glass").gameObject.SetActive(false);
            transform.Find("XBeam").gameObject.SetActive(false);
            transform.Find("ZBeam").gameObject.SetActive(false);
            AudioSource windowAudioSource = transform.Find("Base").GetComponent<AudioSource>();
            windowAudioSource.clip = Assets.Load<AudioClip>("Emotes/JermaWindow/Shatter6.ogg");
            windowAudioSource.Play();
            CustomEmotesAPI.localMapper.StartCoroutine(CreateExplosion());
        }
        public IEnumerator CreateExplosion()
        {
            GameObject g2 = UnityEngine.Object.Instantiate(Assets.Load<GameObject>("Emotes/JermaWindow/Explosion.prefab"));
            g2.transform.SetParent(transform);
            g2.transform.localPosition = new Vector3(0, 1.33f, 0);
            g2.transform.localScale = new Vector3(5, 5, 5);
            g2.transform.SetParent(g2.transform.parent.parent);
            yield return new WaitForSeconds(5);
            g2.GetComponent<ParticleSystem>().Emit(1);
            g2.GetComponent<AudioSource>().Play();
            CustomEmotesAPI.localMapper.StartCoroutine(JermaWindowJoinSpot.CleanupExplosion(g2));
            if (CustomEmotesAPI.localMapper.isServer)
            {
                try
                {
                    GameObject.Destroy(transform.gameObject);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
