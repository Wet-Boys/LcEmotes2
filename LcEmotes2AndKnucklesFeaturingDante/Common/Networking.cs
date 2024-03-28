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
        public void SyncRandomVideoToClientsRpc(ulong netId, int randomNumber)
        {
            GameObject bodyObject = GetNetworkObject(netId).gameObject;
            foreach (var item in BoneMapper.playersToMappers[bodyObject].props)
            {
                VideoPlayer videoPlayer = item.GetComponentInChildren<VideoPlayer>();
                if (videoPlayer is not null)
                {
                    videoPlayer.clip = PhoneEmote.videoClips[randomNumber];
                    videoPlayer.Play();
                }
            }
        }
    }
}
