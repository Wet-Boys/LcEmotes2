using GameNetcodeStuff;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.Megaman;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Video;

namespace LcEmotes2AndKnucklesFeaturingDante.Common
{
    public class Emotes2Networking : Unity.Netcode.NetworkBehaviour
    {
        public static Emotes2Networking instance;
        private void Awake()
        {
            name = "Emotes2_Networker";
            instance = this;
        }
        [ClientRpc]
        public void SyncRandomVideoToClientRpc(ulong netId, int randomNumber, int randomEasterEgg, int useEasterEgg, float randomPercentage)
        {
            GameObject bodyObject = GetNetworkObject(netId).gameObject;
            foreach (var item in BoneMapper.playersToMappers[bodyObject].props)
            {
                VideoPlayer videoPlayer = item.GetComponentInChildren<VideoPlayer>();
                if (videoPlayer is not null)
                {
                    if (useEasterEgg < 600)
                    {
                        videoPlayer.clip = PhoneEmote.easterEggVideoClips[randomEasterEgg];
                    }
                    else
                    {
                        videoPlayer.clip = PhoneEmote.videoClips[randomNumber];
                    }
                    videoPlayer.Play();
                    if (videoPlayer.clip.name == "octagon" || videoPlayer.clip.name == "kitchengun" || videoPlayer.clip.name == "oldspice" || videoPlayer.clip.name == "BadShrekle" || videoPlayer.clip.name == "GreyLeno" || videoPlayer.clip.name == "Lifesaver")
                    {
                        item.transform.localPosition = new Vector3(0.0037f, 0.14f, -0.08f);
                        item.transform.localEulerAngles = new Vector3(70.6643f, 350.0056f, 196.5822f);
                        Transform t = item.transform.Find("Cube/Plane");
                        t.localEulerAngles = new Vector3(0,-90,-90);
                        t.localScale = new Vector3(0.2611268f, 0.1748752f, 0.1791651f);
                    }
                    if (videoPlayer.clip.name == "Lifesaver" || videoPlayer.clip.name == "Megaman" || videoPlayer.clip.name == "True")
                    {
                        videoPlayer.isLooping = false;
                    }
                    else
                    {
                        videoPlayer.time = videoPlayer.clip.length * randomPercentage;
                    }
                }
            }
        }
    }
}
