using EmotesAPI;
using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.LightsCameraAction
{
    internal class CopyRotation : MonoBehaviour
    {
        internal Transform target;
        internal Transform trackedBone;
        internal PlayerControllerB player;
        internal float multiplier;
        void Update()
        {
            this.transform.eulerAngles = new Vector3(target.eulerAngles.x, target.eulerAngles.y, trackedBone.eulerAngles.z) + new Vector3(0, 180, 0);
            this.transform.position = trackedBone.position += new Vector3(target.transform.forward.x * multiplier, target.transform.forward.y * multiplier, target.transform.forward.z * multiplier);
            if (player is not null)
            {
                if (CustomEmotesAPI.ModelReplacementAPIPresent)
                {
                    ModelReplacementAPICompat.UpdateModelRotation(player);
                }
                if (CustomEmotesAPI.VRMPresent)
                {
                    LethalVRMCompat.UpdateModelRotation(player);
                }
            }
        }
    }
}
