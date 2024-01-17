using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Data;

public readonly struct TwitchUsername
{
    public readonly string Username;

    public readonly Color UsernameColor;

    public readonly bool HasSpecialMessage;

    public readonly string? SpecialMessage = null;

    public readonly bool HasPrefabOverride;

    public readonly GameObject? PrefabOverride = null;

    public TwitchUsername(string username, Color usernameColor, string? specialMessage = null, GameObject? prefabOverride = null)
    {
        Username = username;
        UsernameColor = usernameColor;
        HasSpecialMessage = !string.IsNullOrWhiteSpace(specialMessage);
        SpecialMessage = specialMessage;
        HasPrefabOverride = prefabOverride is not null;
        PrefabOverride = prefabOverride;
    }
}