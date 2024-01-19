using UnityEngine;
// ReSharper disable ParameterHidesMember

namespace LcEmotes2AndKnucklesFeaturingDante.Common.Rand;

public abstract class WeightedEntry<TEntry> : ScriptableObject
{
    public float weight;

    [SerializeReference] public TEntry? entry;

    public void Deconstruct(out float weight, out TEntry? entry)
    {
        weight = this.weight;
        entry = this.entry;
    }
}