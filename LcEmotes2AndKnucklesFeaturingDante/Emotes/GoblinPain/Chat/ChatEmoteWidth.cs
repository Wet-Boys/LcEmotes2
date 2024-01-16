using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

public class ChatEmoteWidth : AbstractChatWidth
{
    public float width;
    
    public override float GetWidth() => width;

    public override void SetXPos(float x)
    {
        var t = transform;
        var pos = t.localPosition;
        t.localPosition = new Vector3(x, pos.y, pos.z);
    }
}