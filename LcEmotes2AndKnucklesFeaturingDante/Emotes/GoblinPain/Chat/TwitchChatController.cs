using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Data;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

public class TwitchChatController : MonoBehaviour
{
    public GameObject? chatPrefab;
    public float timeBetweenPulses = 0.25f;

    public RandomTwitchUsernameProvider? twitchUsernameProvider;

    private float _timer;
    
    private void Update()
    {
        _timer -= Time.unscaledDeltaTime;
        
        if (_timer > 0)
            return;
        
        _timer = timeBetweenPulses;
        SpawnChatMessage();
    }

    private void SpawnChatMessage()
    {
        if (twitchUsernameProvider is null)
            return;
        
        var position = transform.position + Random.insideUnitSphere * Random.Range(2f, 6f);
        var chat = Instantiate(chatPrefab, position, Quaternion.AngleAxis(0, Vector3.up));

        if (chat is null)
            return;

        var twitchUsername = twitchUsernameProvider.GetUsername();

        var chatController = chat.GetComponent<ChatMessageController>();
        chatController.SetChat(twitchUsername.Username, twitchUsername.UsernameColor, ":GoblinPls:P:GoblinPls:A:GoblinPls:I:GoblinPls:N:GoblinPls:");
    }
}