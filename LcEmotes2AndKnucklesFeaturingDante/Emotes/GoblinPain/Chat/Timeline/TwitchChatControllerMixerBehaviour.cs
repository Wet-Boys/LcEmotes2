using UnityEngine.Playables;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat.Timeline;

public class TwitchChatControllerMixerBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (playerData is not TwitchChatController controller)
            return;

        var finalWeights = new float[controller.weights.Length];

        int inputCount = playable.GetInputCount();

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            var inputPlayable = (ScriptPlayable<TwitchChatControllerBehaviour>)playable.GetInput(i);
            var input = inputPlayable.GetBehaviour();

            for (int j = 0; j < finalWeights.Length; j++)
                finalWeights[j] += input.weights[j] * inputWeight;
        }

        controller.weights = finalWeights;
    }
}