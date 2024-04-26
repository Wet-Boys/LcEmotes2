using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using GameNetcodeStuff;
using LcEmotes2AndKnucklesFeaturingDante.Common;
using LcEmotes2AndKnucklesFeaturingDante.Emotes;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.HoverEmote;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.JermaWindow;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.Megaman;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.xQcClap;
using LcEmotes2AndKnucklesFeaturingDante.JoinSpots.JermaWindow;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
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
    public const string ModVersion = "1.3.2";
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


        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            try
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }

    private void RegisterAllEmotes()
    {
        EmoteRegistry.RegisterEmote<GoblinPainEmote>();
        EmoteRegistry.RegisterEmote<xQcEmote>();
        EmoteRegistry.RegisterEmote<xQcEmote_yell>();
        EmoteRegistry.RegisterEmote<xQcEmote_clap>();
        //EmoteRegistry.RegisterEmote<MegamanEmote>();
        EmoteRegistry.RegisterEmote<LightsCameraActionEmote>();
        EmoteRegistry.RegisterEmote<PhoneEmote>();
        EmoteRegistry.RegisterEmote<JermaWindow>();
        EmoteRegistry.RegisterEmote<JermaWindowJump>();
        EmoteRegistry.RegisterEmote<JermaWindowLand>();
        EmoteRegistry.RegisterEmote<JermaWindowFall>();
        EmoteRegistry.RegisterEmote<HoverEmote>();
        Assets.Load<GameObject>("Emotes/JermaWindow/Window9.prefab").AddComponent<WindowHandler>();
        EmoteRegistry.RegiserProp(JermaWindow.props, Assets.Load<GameObject>("Emotes/JermaWindow/Window9.prefab"), [new JoinSpot("JermaWindowSpot", new Vector3(0, 0, 2.5f))], [new JermaWindowJoinSpot()]);

    }
}