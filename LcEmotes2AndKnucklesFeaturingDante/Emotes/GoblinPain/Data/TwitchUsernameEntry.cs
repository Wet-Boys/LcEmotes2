using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Data;

[CreateAssetMenu(fileName = "TwitchUsername", menuName = "Emotes 2/Twitch/Username")]
public class TwitchUsernameEntry : ScriptableObject, ITwitchUsernameProvider
{
    public string username = "";
    public Color usernameColor;
    public string specialMessage = "";
    public GameObject? prefabOverride;
    
    public TwitchUsername GetUsername() => new(username, usernameColor, specialMessage, prefabOverride);
}