using LcEmotes2AndKnucklesFeaturingDante.Common.Rand;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Data;

[CreateAssetMenu(fileName = "TwitchUsernameList", menuName = "Emotes 2/Twitch/Username List")]
public class TwitchUsernameList : WeightedRandomData<TwitchUsernameEntry>, ITwitchUsernameProvider
{
    public TwitchUsername GetUsername()
    {
        var entry = GetRandomEntry();

        if (!entry.TryGetValue(out var value))
            return default;

        if (value is null)
            return default;

        return value.GetUsername();
    }
}