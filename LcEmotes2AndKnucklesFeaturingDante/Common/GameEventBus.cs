using System;
using LcEmotes2AndKnucklesFeaturingDante.Utils;
using MonoMod.RuntimeDetour;

namespace LcEmotes2AndKnucklesFeaturingDante.Common;

internal static class GameEventBus
{
    public static void InitHooks()
    {
        _radiationWarningHUDHook = HookUtils.NewHook<HUDManager>(nameof(HUDManager.RadiationWarningHUD),
            typeof(GameEventBus), nameof(RadiationWarningHUD));
    }

    private static Hook? _radiationWarningHUDHook;

    public static event Action? OnRadiationWarningHUD;
    private static void RadiationWarningHUD(Action<HUDManager> orig, HUDManager self)
    {
        orig(self);
        
        OnRadiationWarningHUD?.Invoke();
    }
}