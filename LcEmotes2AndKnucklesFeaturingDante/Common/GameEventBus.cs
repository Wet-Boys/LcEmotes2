using System;
using EmotesAPI;
using GameNetcodeStuff;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.JermaWindow;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.LightsCameraAction;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.Megaman;
using LcEmotes2AndKnucklesFeaturingDante.Utils;
using MonoMod.RuntimeDetour;
using Unity.Netcode;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Common;

internal static class GameEventBus
{
    public static void InitHooks()
    {
        _radiationWarningHUDHook = HookUtils.NewHook<HUDManager>(nameof(HUDManager.RadiationWarningHUD),
            typeof(GameEventBus), nameof(RadiationWarningHUD), false);
        _switchSuitForPlayerHook = HookUtils.NewHook<UnlockableSuit>(nameof(UnlockableSuit.SwitchSuitForPlayer),
            typeof(GameEventBus), nameof(SwitchSuitForPlayer), true);

        _radiationWarningHUDHook = HookUtils.NewHook<PlayerControllerB>("Start",
            typeof(GameEventBus), nameof(PlayerControllerStart), false);
        _switchSuitForPlayerHook = HookUtils.NewHook<GameNetworkManager>("Start",
            typeof(GameEventBus), nameof(NetworkManagerStart), false);

        shootGunHook = HookUtils.NewHook<ShotgunItem>("ShootGun",
            typeof(GameEventBus), nameof(ShootGun), false);
    }

    private static Hook? _radiationWarningHUDHook;
    private static Hook? _switchSuitForPlayerHook;
    private static Hook? networkManagerStartHook;
    private static Hook? playerControllerStartHook;
    private static Hook? shootGunHook;


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
    private static GameObject emotes2networker;
    private static void PlayerControllerStart(Action<PlayerControllerB> orig, PlayerControllerB self)
    {
        orig(self);
        if (self.IsServer && Emotes2Networking.instance == null)
        {
            GameObject networker = UnityEngine.Object.Instantiate<GameObject>(emotes2networker);
            networker.GetComponent<NetworkObject>().Spawn(true);
        }
    }
    private static void NetworkManagerStart(Action<GameNetworkManager> orig, GameNetworkManager self)
    {
        try
        {
            emotes2networker = Assets.Load<GameObject>($"assets/emotes/networkerlmao.prefab");

            emotes2networker.AddComponent<Emotes2Networking>();
            GameNetworkManager.Instance.GetComponent<NetworkManager>().PrefabHandler.AddNetworkPrefab(emotes2networker);
        }
        catch (Exception e)
        {
            DebugClass.Log($"couldn't setup emotes2 networker");
        }
        orig(self);
    }
    private static void ShootGun(Action<ShotgunItem, Vector3, Vector3> orig, ShotgunItem self, Vector3 shotgunPosition, Vector3 shotgunForward)
    {
        orig(self, shotgunPosition, shotgunForward);
        try
        {
            foreach (var item in self.enemyColliders)
            {
                if (item.transform is not null && item.transform.name == "Window9(Clone)")
                {
                    item.transform.GetComponent<WindowHandler>().OnHit();
                }
            }
        }
        catch (Exception)
        {
        }
    }
}