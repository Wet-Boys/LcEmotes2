using Emotes2JigglePhysics;
using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.JermaWindow;
using LethalEmotesAPI.ImportV2;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.MottekeSailorFukku;

public class MottekeSailorFukkuStage : AbstractEmote
{
    public static List<int> props = new List<int>();
    public static GameObject currentStage;
    public static GameObject currentCamera;
    public static AudioSource stageAudio;
    public static BoneMapper[] dancers = [null, null, null, null, null, null, null, null, null, null];
    public override string AnimationClipName => "spawnStage";

    public override CustomEmoteParams GetClipParams()
    {
        return new CustomEmoteParams
        {
            internalName = "spawnStage",
            displayName = "Motteke! Sailor Fuku",
        };
    }

    public override void SpawnProps(BoneMapper mapper)
    {
        if (CustomEmotesAPI.localMapper.isServer)
        {
            if (currentStage is not null)
            {
                foreach (var item in dancers)
                {
                    if (item is not null)
                    {
                        return;
                    }
                }
                GameObject.Destroy(currentCamera);
                GameObject.Destroy(currentStage);
                currentCamera = null;
                currentStage = null;
            }
            GameObject g = CustomEmotesAPI.SpawnWorldProp(props[0]);
            g.transform.position = mapper.mapperBody.transform.position;
            g.transform.position += (mapper.mapperBody.transform.forward * 1.2f) + new Vector3(0,.2f,0);
            g.transform.eulerAngles = mapper.mapperBody.transform.eulerAngles + new Vector3(0, 180, 0);
            g.transform.localScale = new Vector3(.1f, .1f, .1f);
            g.GetComponent<NetworkObject>().Spawn();
            if (mapper.mapperBodyTransform.parent.name == "HangarShip")
            {
                g.transform.SetParent(mapper.mapperBodyTransform.parent);
            }
        }
    }
}