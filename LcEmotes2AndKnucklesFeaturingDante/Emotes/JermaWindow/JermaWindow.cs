using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Playables;
using UnityEngine;
using System.Collections;
using UnityEngine.Timeline;
using LethalEmotesAPI.ImportV2;
using UnityEngine.UIElements;
using Unity.Netcode;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.JermaWindow
{
    public class JermaWindow : AbstractEmote
    {
        public override string AnimationClipName => "jermaWindow";
        public static List<int> props = new List<int>();
        public override CustomEmoteParams GetClipParams()
        {
            return new CustomEmoteParams
            {
                internalName = "jermaWindow",
                displayName = "jermaWindow"
            };
        }

        public override void SpawnProps(BoneMapper mapper)
        {
            if (CustomEmotesAPI.localMapper.isServer)
            {
                foreach (var item in WindowHandler.allInactiveWindows)
                {
                    if (item.owner == mapper)
                    {
                        GameObject.Destroy(item.gameObject);
                    }
                }
                GameObject g = CustomEmotesAPI.SpawnWorldProp(props[0]);
                g.transform.position = mapper.mapperBody.transform.position;
                g.transform.position += mapper.mapperBody.transform.forward * 2;
                g.transform.eulerAngles = mapper.mapperBody.transform.eulerAngles + new Vector3(0, 180, 0);
                g.GetComponent<WindowHandler>().owner = mapper;
                g.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
