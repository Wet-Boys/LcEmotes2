using UnityEngine.Playables;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

public class TwitchChatControllerBehaviour : PlayableBehaviour
{
    // ReSharper disable InconsistentNaming
    public TwitchChatController? controller = null;

    public float[] weights = [];
    // ReSharper restore InconsistentNaming

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (controller is null)
            return;

        if (weights.Length != controller.weights.Length)
            return;

        for (int i = 0; i < controller.weights.Length; i++)
        {
            var newWeight = weights[i];
            
            if (newWeight == 0f)
            {
                var oldWeight = controller.weights[i];
                controller.weights[i] = oldWeight * (1 - info.effectiveWeight);
                continue;
            }
            
            controller.weights[i] = newWeight * info.effectiveWeight;
        }
    }
}