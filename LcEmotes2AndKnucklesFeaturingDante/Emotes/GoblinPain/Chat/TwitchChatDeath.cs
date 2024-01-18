﻿using GameNetcodeStuff;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

public class TwitchChatDeath : MonoBehaviour
{
    public PlayerControllerB? ownerPlayer;
    public GameObject? deadPrefab;
    
    private void OnDestroy()
    {
        if (deadPrefab is null || ownerPlayer is null)
            return;

        if (!ownerPlayer.isPlayerDead)
            return;

        Instantiate(deadPrefab, transform.position, Quaternion.AngleAxis(0, Vector3.up));
    }
}