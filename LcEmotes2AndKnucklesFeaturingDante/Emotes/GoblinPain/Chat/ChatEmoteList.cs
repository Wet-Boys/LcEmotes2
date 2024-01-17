using System.Collections.Generic;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

[CreateAssetMenu(fileName = "ChatEmoteList", menuName = "Emotes 2/Twitch/Chat Emote List")]
public class ChatEmoteList : ScriptableObject
{
    public ChatEmoteEntry[] emoteEntries = [];

    private readonly Dictionary<string, GameObject?> _emoteLut = [];

    private string[]? _emoteNames;

    public string[] EmoteNames
    {
        get
        {
            if (_emoteNames is not null)
                return _emoteNames;
            
            _emoteNames = new string[emoteEntries.Length];
            
            for (int i = 0; i < _emoteNames.Length; i++)
                _emoteNames[i] = emoteEntries[i].emoteName;

            return _emoteNames;
        }
    }

    private string[]? _emoteKeys;

    public string[] EmoteKeys
    {
        get
        {
            if (_emoteKeys is not null)
                return _emoteKeys;

            _emoteKeys = new string[emoteEntries.Length];

            for (int i = 0; i < _emoteKeys.Length; i++)
                _emoteKeys[i] = $":{emoteEntries[i].emoteName}:";

            return _emoteKeys;
        }
    }

    private void Awake()
    {
        _emoteLut.Clear();

        foreach (var entry in emoteEntries)
            _emoteLut[entry.emoteName] = entry.prefab;
    }

    public GameObject? this[string emoteName] => _emoteLut.GetValueOrDefault(emoteName);
}