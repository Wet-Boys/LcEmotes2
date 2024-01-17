using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Data;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

public class TwitchChatController : MonoBehaviour
{
    public GameObject? chatPrefab;
    public float timeBetweenPulses = 0.25f;

    public RandomTwitchUsernameProvider? twitchUsernameProvider;

    public float[] weights = [];
    public string[] messages = [];

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
        var twitchUsername = twitchUsernameProvider.GetUsername();

        GameObject? chat;
        
        if (twitchUsername is { HasPrefabOverride: true, PrefabOverride: not null })
            chat = Instantiate(twitchUsername.PrefabOverride, position, Quaternion.AngleAxis(0, Vector3.up));
        else
            chat = Instantiate(chatPrefab, position, Quaternion.AngleAxis(0, Vector3.up));
        
        if (chat is null)
            return;
        
        var chatController = chat.GetComponent<ChatMessageController>();

        string message;

        if (twitchUsername.HasSpecialMessage && !string.IsNullOrWhiteSpace(twitchUsername.SpecialMessage))
            message = twitchUsername.SpecialMessage;
        else
            message = GetRandomMessage();
        
        chatController.SetChat(twitchUsername.Username, twitchUsername.UsernameColor, message);
    }
    
    private string GetRandomMessage()
    {
        if (weights.Length != messages.Length)
            return "";
        
        if (weights.Length == 0)
            return "";

        float sumOfWeights = 0;

        for (var i = 0; i < weights.Length; i++)
        {
            var weight = weights[i];

            if (float.IsPositiveInfinity(weight))
                return messages[i];

            if (weight >= 0 && !float.IsNaN(weight))
                sumOfWeights += weight;
        }

        var randomValue = Random.value;
        var currentSum = 0f;

        for (var i = 0; i < weights.Length; i++)
        {
            var weight = weights[i];

            if (float.IsNaN(weight) || weight <= 0f)
                continue;

            currentSum += weight / sumOfWeights;

            if (currentSum >= randomValue)
                return messages[i];
        }

        return "";
    }
}