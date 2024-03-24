using System;
using EmotesAPI;
using GameNetcodeStuff;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.LightsCameraAction;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.Megaman;
using LcEmotes2AndKnucklesFeaturingDante.Utils;
using MonoMod.RuntimeDetour;

namespace LcEmotes2AndKnucklesFeaturingDante.Common;

internal static class GameEventBus
{
    public static void InitHooks()
    {
        _radiationWarningHUDHook = HookUtils.NewHook<HUDManager>(nameof(HUDManager.RadiationWarningHUD),
            typeof(GameEventBus), nameof(RadiationWarningHUD), false);
        _switchSuitForPlayerHook = HookUtils.NewHook<UnlockableSuit>(nameof(UnlockableSuit.SwitchSuitForPlayer),
            typeof(GameEventBus), nameof(SwitchSuitForPlayer), true);
    }

    private static Hook? _radiationWarningHUDHook;
    private static Hook? _switchSuitForPlayerHook;

    public static event Action? OnRadiationWarningHUD;
    private static void RadiationWarningHUD(Action<HUDManager> orig, HUDManager self)
    {
        orig(self);

        OnRadiationWarningHUD?.Invoke();
    }
    private static void SwitchSuitForPlayer(Action<PlayerControllerB, int, bool> orig, PlayerControllerB player, int suitID, bool playAudio = true)
    {
        if (CustomEmotesAPI.ModelReplacementAPIPresent)
        {
            ModelReplacementAPICompat.RemovePlayerFromPool(player);
        }
        if (CustomEmotesAPI.VRMPresent)
        {
            LethalVRMCompat.RemovePlayerFromPool(player);
        }
        orig(player, suitID, playAudio);
        if (BoneMapper.playersToMappers.ContainsKey(player.gameObject))
        {
            if (BoneMapper.playersToMappers[player.gameObject].currentClipName == "com.gemumoddo.lc_emotes2_and_knuckles_featuring_dante__LightsCameraActionStart")
            {
                BoneMapper.playersToMappers[player.gameObject].StartCoroutine(LightsCameraActionEmote.FinishPropAfterFrame(BoneMapper.playersToMappers[player.gameObject]));
            }
        }
    }
}