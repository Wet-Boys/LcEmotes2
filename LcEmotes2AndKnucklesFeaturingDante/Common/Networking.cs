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
        public void SyncRandomVideoToClientRpc(ulong netId, int randomNumber, int randomEasterEgg, int useEasterEgg)
        {
            GameObject bodyObject = GetNetworkObject(netId).gameObject;
            foreach (var item in BoneMapper.playersToMappers[bodyObject].props)
            {
                VideoPlayer videoPlayer = item.GetComponentInChildren<VideoPlayer>();
                if (videoPlayer is not null)
                {
                    if (useEasterEgg < 5)
                    {
                        videoPlayer.clip = PhoneEmote.easterEggVideoClips[randomEasterEgg];
                    }
                    else
                    {
                        videoPlayer.clip = PhoneEmote.videoClips[randomNumber];
                    }
                    videoPlayer.Play();
                }
            }
        }
    }
}
