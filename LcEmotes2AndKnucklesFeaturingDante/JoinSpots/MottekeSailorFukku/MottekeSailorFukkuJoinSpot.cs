using EmotesAPI;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.JermaWindow;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.MottekeSailorFukku;
using LethalEmotesAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
// using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
using Emotes2JigglePhysics;

namespace LcEmotes2AndKnucklesFeaturingDante.JoinSpots.MottekeSailorFukku
{
    public class MottekeSailorFukkuJoinSpot : AbstractJoinSpot
    {
        public override void OnSpotJoined(GameObject emoteSpot, BoneMapper joiner, BoneMapper host)
        {
            if (!MottekeSailorFukkuStage.stageAudio)
            {
                return;
            }
            int i = 0;
            for (; i < 10; i++)
            {
                if (MottekeSailorFukkuStage.dancers[i] is null)
                {
                    MottekeSailorFukkuStage.dancers[i] = joiner;
                    if (i == 0 && !MottekeSailorFukkuStage.stageAudio.isPlaying)
                    {
                        MottekeSailorFukkuStage.stageAudio.volume = (Settings.EmotesVolume.Value / 100f) * .4f;
                        MottekeSailorFukkuStage.stageAudio.Play();
                    }
                    break;
                }
            }
            MottekeSailorFukkuStage.stageAudio.mute = Settings.DMCAFree.Value == DMCAType.AllOff || Settings.DMCAFree.Value == DMCAType.Mute || Settings.DMCAFree.Value == DMCAType.Friendly;
            if (i == 9)
            {
                emoteSpot.transform.localPosition += new Vector3(5000, 5000, 5000);
            }
            joiner.mapperBodyTransform.position = MottekeSailorFukkuStage.currentStage.transform.GetChild(i + 1).position;
            joiner.mapperBodyTransform.rotation = MottekeSailorFukkuStage.currentStage.transform.GetChild(i + 1).rotation;
            joiner.PlayAnim($"{LcEmotes2AndKnucklesFeaturingDantePlugin.ModGuid}__luckystarDance{i+1}", -1);
            GameObject pomPom1 = GameObject.Instantiate(Assets.Load<GameObject>($"Emotes/Lucky Star/Pom Poms/pom pom test 3.prefab"));
            GameObject pomPom2 = GameObject.Instantiate(Assets.Load<GameObject>($"Emotes/Lucky Star/Pom Poms/pom pom test 3.prefab"));
            SetupJiggle(pomPom1);
            SetupJiggle(pomPom2);
            joiner.props.Add(pomPom1);
            joiner.props.Add(pomPom2);
            pomPom1.transform.parent = joiner.emoteSkeletonAnimator.GetBoneTransform(HumanBodyBones.LeftHand);
            pomPom2.transform.parent = joiner.emoteSkeletonAnimator.GetBoneTransform(HumanBodyBones.RightHand);
            pomPom1.transform.localPosition = new Vector3(-0.004f, 0.082f, -0.004f);
            pomPom2.transform.localPosition = new Vector3(-0.0008f, 0.0577f, -0.0027f);




            joiner.AssignParentGameObject(MottekeSailorFukkuStage.currentStage.transform.GetChild(i + 1).gameObject, true, false, true, true, false);
            joiner.props.Add(new GameObject());
            MottekeSailorFukkuCleanup cleanup = joiner.props.Last().AddComponent<MottekeSailorFukkuCleanup>();
            cleanup.mapper = joiner;
            cleanup.spotInArray = i;
            cleanup.emoteSpot = emoteSpot;
            if (joiner.local)
            {
                DanceFreeCam.instance.active = true;
                HUDManager.Instance.SetNearDepthOfFieldEnabled(false);
                // DepthOfField depthOfField;
                // if (HUDManager.Instance.playerGraphicsVolume.sharedProfile.TryGet<DepthOfField>(out depthOfField))
                // {
                //     depthOfField.nearFocusEnd.SetValue(new MinFloatParameter(.05f, 0f, true));
                // }
                MottekeSailorFukkuStage.stageAudio.spatialBlend = 0;
                EmoteLocation yote = emoteSpot.GetComponent<EmoteLocation>();
                try
                {
                    foreach (var item2 in yote.GetComponentsInChildren<Renderer>())
                    {
                        item2.enabled = false;
                    }
                    foreach (var item2 in yote.GetComponentsInChildren<ParticleSystemRenderer>())
                    {
                        item2.enabled = false;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void SetupJiggle(GameObject gameObject)
        {
            JiggleRigBuilder jiggleBone = gameObject.AddComponent<JiggleRigBuilder>();
            JiggleSettings jiggleSettings = ScriptableObject.CreateInstance<JiggleSettings>();
            JiggleSettingsData data = new JiggleSettingsData();
            data.gravityMultiplier = .04f;
            data.friction = .096f;
            data.angleElasticity = .562f;
            data.blend = .35f;
            data.airDrag = 0;
            data.lengthElasticity = 1;
            data.elasticitySoften = 1;
            jiggleSettings.SetData(data);
            jiggleBone.jiggleRigs =
            [
                new JiggleRigBuilder.JiggleRig(jiggleBone.transform.GetChild(0).GetChild(0).GetChild(0), jiggleSettings, [], []),
            ];
        }
    }
}
