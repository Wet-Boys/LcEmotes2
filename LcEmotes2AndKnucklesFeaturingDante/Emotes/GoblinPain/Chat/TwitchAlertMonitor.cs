
using System.Collections;
using LcEmotes2AndKnucklesFeaturingDante.Common;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

public class TwitchAlertMonitor : MonoBehaviour
{
    public TwitchChatController? chatController;

    private IEnumerator? _stopAlertCoroutine;

    private void Awake()
    {
        GameEventBus.OnRadiationWarningHUD += OnRadiationWarning;
    }

    private void OnDestroy()
    {
        GameEventBus.OnRadiationWarningHUD -= OnRadiationWarning;
    }

    private void OnRadiationWarning()
    {
        if (chatController is null)
            return;
        
        chatController.alertActive = true;
        
        if (_stopAlertCoroutine is not null)
        {
            StopCoroutine(_stopAlertCoroutine);
            _stopAlertCoroutine = null;
        }

        _stopAlertCoroutine = StopAlert();
        StartCoroutine(_stopAlertCoroutine);
    }

    private IEnumerator StopAlert()
    {
        yield return new WaitForSeconds(5f);

        if (chatController is null)
            yield break;

        chatController.alertActive = false;
    }
}