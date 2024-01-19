using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

[CreateAssetMenu(fileName = "EmoteEntry", menuName = "Emotes 2/Twitch/Emote Entry")]
public class ChatEmoteEntry : ScriptableObject
{
    public string emoteName = "";
    public GameObject? prefab;
}