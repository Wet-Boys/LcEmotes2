using System.Linq;
using UnityEngine.Playables;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat.Timeline;

public class TwitchChatControllerMixerBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (playerData is not TwitchChatController controller)
            return;

        var fearIndices = controller.fearIndices;
        var alertIndices = controller.alertIndices;
        var finalWeights = new float[controller.weights.Length];

        int inputCount = playable.GetInputCount();

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            var inputPlayable = (ScriptPlayable<TwitchChatControllerBehaviour>)playable.GetInput(i);
            var input = inputPlayable.GetBehaviour();

            for (int j = 0; j < finalWeights.Length; j++)
            {
                if (fearIndices.Contains(j) || alertIndices.Contains(j))
                    continue;
                
                finalWeights[j] += input.weights[j] * inputWeight;
            }

            foreach (var f in controller.fearIndices)
                finalWeights[f] += controller.fearWeight * inputWeight * controller.fearFactor;

            var alertActive = controller.alertActive ? 1f : 0f;
            foreach (var a in controller.alertIndices)
                finalWeights[a] += controller.alertWeight * inputWeight * alertActive;
        }

        controller.weights = finalWeights;
    }
}