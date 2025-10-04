using System;
using LethalVRM;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GameNetcodeStuff;
using static LethalVRM.LethalVRMManager;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.LightsCameraAction
{
    internal class LethalVRMCompat
    {
        internal static Dictionary<PlayerControllerB, object> playersToVRMInstances = new();

        internal static GameObject iWantToSeeYourManager;
        internal static void SetManager()
        {
            if (iWantToSeeYourManager is null)
            {
                foreach (var item in Resources.FindObjectsOfTypeAll(typeof(LethalVRMManager)) as LethalVRMManager[])
                {
                    iWantToSeeYourManager = item.gameObject;
                }
            }
        }
        internal static void GetPlayerInDictionary(PlayerControllerB player)
        {
            if (iWantToSeeYourManager is not null)
            {
                if (!playersToVRMInstances.ContainsKey(player))
                {
                    foreach (var item in iWantToSeeYourManager.GetComponent<LethalVRMManager>().instances)
                    {
                        if (item.PlayerControllerB == player)
                        {
                            playersToVRMInstances.Add(item.PlayerControllerB, item);
                        }
                    }
                }
            }
        }
        internal static LethalVRMInstance PerformAllChecks(PlayerControllerB player)
        {
            SetManager();
            GetPlayerInDictionary(player);
            if (playersToVRMInstances.ContainsKey(player))
            {
                return (LethalVRMInstance)playersToVRMInstances[player];
            }
            return null;
        }
        internal static GameObject ChangeModelScale(bool makeNormal, PlayerControllerB player)
        {
            LethalVRMInstance bod = PerformAllChecks(player);
            if (bod is not null)
            {
                if (makeNormal)
                {
                    if (playersToVRMInstances.ContainsKey(player))
                    {
                        playersToVRMInstances.Remove(player);
                    }
                    bod.Vrm10Instance.transform.localScale = new Vector3(bod.Vrm10Instance.transform.localScale.z, bod.Vrm10Instance.transform.localScale.y, bod.Vrm10Instance.transform.localScale.z);
                }
                else
                {
                    if (!playersToVRMInstances.ContainsKey(player))
                    {
                        playersToVRMInstances.Add(player, bod);
                    }
                    bod.Vrm10Instance.transform.localScale = new Vector3(.001f, bod.Vrm10Instance.transform.localScale.y, bod.Vrm10Instance.transform.localScale.z);
                }
                return bod.Vrm10Instance.gameObject;
            }
            return null;
        }
        internal static void UpdateModelRotation(PlayerControllerB player)
        {
            LethalVRMInstance bod = PerformAllChecks(player);
            if (bod is not null)
            {
                bod.Vrm10Instance.transform.eulerAngles = player.transform.eulerAngles + new Vector3(0, 90, 0);
            }
        }
        internal static void RemovePlayerFromPool(PlayerControllerB player)
        {
            LethalVRMCompat.playersToVRMInstances.Remove(player);
        }
    }
}
