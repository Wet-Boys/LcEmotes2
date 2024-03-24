using GameNetcodeStuff;
using ModelReplacement;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.LightsCameraAction
{
    internal class ModelReplacementAPICompat
    {
        internal static Dictionary<PlayerControllerB, BodyReplacementBase> playersToBodies = new Dictionary<PlayerControllerB, BodyReplacementBase>();
        internal static GameObject ChangeModelScale(bool makeNormal, PlayerControllerB player)
        {
            BodyReplacementBase bod;
            ModelReplacementAPI.GetPlayerModelReplacement(player, out bod);
            if (bod is not null)
            {
                if (makeNormal)
                {
                    if (playersToBodies.ContainsKey(player))
                    {
                        playersToBodies.Remove(player);
                    }
                    bod.replacementModel.transform.localScale = new Vector3(bod.replacementModel.transform.localScale.z, bod.replacementModel.transform.localScale.y, bod.replacementModel.transform.localScale.z);
                }
                else
                {
                    if (!playersToBodies.ContainsKey(player))
                    {
                        playersToBodies.Add(player, bod);
                    }
                    bod.replacementModel.transform.localScale = new Vector3(.001f, bod.replacementModel.transform.localScale.y, bod.replacementModel.transform.localScale.z);
                }
                return bod.replacementModel;
            }
            return null;
        }
        internal static void UpdateModelRotation(PlayerControllerB player)
        {
            if (playersToBodies.ContainsKey(player))
            {
                playersToBodies[player].replacementModel.transform.eulerAngles = player.transform.eulerAngles + new Vector3(0, 90, 0);
            }
        }
        internal static void RemovePlayerFromPool(PlayerControllerB player)
        {
            playersToBodies.Remove(player);
        }
    }
}
