using EmotesAPI;
using LethalEmotesAPI.ImportV2;
using System.Collections.Generic;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes;

public abstract class AbstractEmote
{
    public abstract string AnimationClipName { get; }

    public abstract CustomEmoteParams GetClipParams();

    public virtual void SpawnProps(BoneMapper mapper) { }
}