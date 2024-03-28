﻿using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using GameNetcodeStuff;
using LcEmotes2AndKnucklesFeaturingDante.Common;
using LcEmotes2AndKnucklesFeaturingDante.Emotes;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.JermaWindow;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.Megaman;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.xQcClap;
using LcEmotes2AndKnucklesFeaturingDante.JoinSpots.JermaWindow;
using MonoMod.RuntimeDetour;
using System;
using Unity.Netcode;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante;

[BepInDependency("com.weliveinasociety.CustomEmotesAPI")]
[BepInDependency("flowerwater.liquidStainRemover", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin(ModGuid, ModName, ModVersion)]
public class LcEmotes2AndKnucklesFeaturingDantePlugin : BaseUnityPlugin
{
    public const string ModGuid = "com.gemumoddo.lc_emotes2_and_knuckles_featuring_dante";
    public const string ModName = "Emotes 2 And Knuckles Featuring Dante";
    public const string ModVersion = "1.1.1";
    public static bool watermarkRemoverPresent = false;
    public static PluginInfo? PluginInfo { get; private set; }
    public new static ManualLogSource? Logger { get; private set; }


    private void Awake()
    {
        PluginInfo = Info;
        Logger = base.Logger;
        watermarkRemoverPresent = Chainloader.PluginInfos.ContainsKey("flowerwater.liquidStainRemover");
        Assets.LoadAllAssetBundles();

        RegisterAllEmotes();
        
        EmoteRegistry.FinalizeRegistry();
        
        GameEventBus.InitHooks();


        //var targetMethod = typeof(PlayerControllerB).GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //var destMethod = typeof(LcEmotes2AndKnucklesFeaturingDantePlugin).GetMethod(nameof(PlayerControllerStart), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //playerControllerStartHook = new Hook(targetMethod, destMethod, this);

        //targetMethod = typeof(GameNetworkManager).GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //destMethod = typeof(LcEmotes2AndKnucklesFeaturingDantePlugin).GetMethod(nameof(NetworkManagerStart), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //networkManagerStartHook = new Hook(targetMethod, destMethod, this);
    }

    private void RegisterAllEmotes()
    {
        EmoteRegistry.RegisterEmote<GoblinPainEmote>();
        EmoteRegistry.RegisterEmote<xQcEmote>();
        EmoteRegistry.RegisterEmote<xQcEmote_yell>();
        EmoteRegistry.RegisterEmote<xQcEmote_clap>();
        //EmoteRegistry.RegisterEmote<JermaWindow>();
        //EmoteRegistry.RegisterEmote<MegamanEmote>();
        EmoteRegistry.RegisterEmote<LightsCameraActionEmote>();
        EmoteRegistry.RegisterEmote<PhoneEmote>();
        //Assets.Load<GameObject>("Emotes/JermaWindow/Window5.prefab").AddComponent<WindowHandler>();
        //EmoteRegistry.RegiserProp(JermaWindow.props, Assets.Load<GameObject>("Emotes/JermaWindow/Window5.prefab"), [new JoinSpot("JermaWindowSpot", new Vector3(0, 0, 2))], [new JermaWindowJoinSpot()]);

    }
}