using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

public class TwitchChatController : MonoBehaviour
{
    public GameObject? chatPrefab;
    public float timeBetweenPulses = 0.25f;

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
        var position = transform.position + Random.insideUnitSphere * Random.Range(2f, 6f);
        var chat = Instantiate(chatPrefab, position, Quaternion.AngleAxis(0, Vector3.up));

        if (chat is null)
            return;

        var chatController = chat.GetComponent<ChatMessageController>();
        chatController.SetChat("Rune580", Color.red, ":GoblinPls:P:GoblinPls:A:GoblinPls:I:GoblinPls:N:GoblinPls:");
    }
}