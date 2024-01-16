using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Data;

public readonly struct TwitchUsername
{
    public readonly string Username;

    public readonly Color UsernameColor;

    public readonly bool HasSpecialMessage;

    public TwitchUsername(string username, Color usernameColor, bool hasSpecialMessage = false)
    {
        Username = username;
        UsernameColor = usernameColor;
        HasSpecialMessage = hasSpecialMessage;
    }
}