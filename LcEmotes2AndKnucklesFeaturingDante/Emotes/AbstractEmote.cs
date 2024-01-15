using EmotesAPI;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes;

public abstract class AbstractEmote
{
    public abstract string AnimationClipName { get; }

    public abstract AnimationClipParams GetClipParams();

    public virtual void SpawnProps(BoneMapper mapper) { }
}