using LcEmotes2AndKnucklesFeaturingDante.Common.Rand;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Data;

// Yes I know this is hacky, leave me alone - Rune580
[CreateAssetMenu(fileName = "RandomTwitchUsernameProvider", menuName = "Emotes 2/Twitch/Random Username Provider")]
public class RandomTwitchUsernameProvider : WeightedRandomData<ScriptableObject>, ITwitchUsernameProvider
{
    public TwitchUsername GetUsername()
    {
        var entry = GetRandomEntry();
        
        if (!entry.TryGetValue(out var value))
            return default;

        if (value is not ITwitchUsernameProvider usernameProvider)
            return default;

        return usernameProvider.GetUsername();
    }
}