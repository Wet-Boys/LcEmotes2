﻿using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Data;

[CreateAssetMenu(fileName = "TwitchUsername", menuName = "Emotes 2/Twitch/Username")]
public class TwitchUsernameEntry : ScriptableObject, ITwitchUsernameProvider
{
    public string username = "";
    public Color usernameColor;
    public bool hasSpecialMessage;
    
    public TwitchUsername GetUsername() => new(username, usernameColor, hasSpecialMessage);
}