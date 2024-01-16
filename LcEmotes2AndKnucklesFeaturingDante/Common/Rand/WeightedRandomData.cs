using LcEmotes2AndKnucklesFeaturingDante.Utils;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Common.Rand;

public abstract class WeightedRandomData<TEntry> : ScriptableObject
{
    public WeightedEntry<TEntry>[] weightedEntries = [];

    public Optional<TEntry> GetRandomEntry()
    {
        if (weightedEntries.Length == 0)
            return Optional<TEntry>.None();

        float sumOfWeights = 0;
        
        foreach (var (weight, entry) in weightedEntries)
        {
            if (entry is null)
                continue;

            if (float.IsPositiveInfinity(weight))
                return Optional<TEntry>.Some(entry);

            if (weight >= 0 && !float.IsNaN(weight))
                sumOfWeights += weight;
        }

        var randomValue = Random.value;
        var currentSum = 0f;

        foreach (var (weight, entry) in weightedEntries)
        {
            if (entry is null)
                continue;

            if (float.IsNaN(weight) || weight <= 0f)
                continue;

            currentSum += weight / sumOfWeights;

            if (currentSum >= randomValue)
                return Optional<TEntry>.Some(entry);
        }
        
        return Optional<TEntry>.None();
    }
}