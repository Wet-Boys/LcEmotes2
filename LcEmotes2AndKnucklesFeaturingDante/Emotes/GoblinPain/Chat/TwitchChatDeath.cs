using GameNetcodeStuff;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

public class TwitchChatDeath : MonoBehaviour
{
    public PlayerControllerB? ownerPlayer;
    public GameObject? deadPrefab;
    float timer = 0f;
    Vector3 desiredPostion;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > .5f && ownerPlayer is not null && !ownerPlayer.isPlayerDead)
        {
            timer = 0f;
            desiredPostion = transform.position;
        }
    }
    
    private void OnDestroy()
    {
        if (deadPrefab is null || ownerPlayer is null)
            return;


        if (!ownerPlayer.isPlayerDead)
            return;

        Instantiate(deadPrefab, desiredPostion, Quaternion.AngleAxis(0, Vector3.up));
    }
}