using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

public abstract class AbstractChatWidth : MonoBehaviour
{
    public abstract float GetWidth();

    public abstract void SetXPos(float x);

    public virtual void UpdateState() { }
}