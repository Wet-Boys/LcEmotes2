﻿using BepInEx;
using BepInEx.Logging;
using LcEmotes2AndKnucklesFeaturingDante.Emotes;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain;

namespace LcEmotes2AndKnucklesFeaturingDante;

[BepInDependency("com.weliveinasociety.CustomEmotesAPI")]
[BepInPlugin(ModGuid, ModName, ModVersion)]
public class LcEmotes2AndKnucklesFeaturingDantePlugin : BaseUnityPlugin
{
    public const string ModGuid = "com.gemumoddo.lc_emotes2_and_knuckles_featuring_dante";
    public const string ModName = "Emotes 2 And Knuckles Featuring Dante";
    public const string ModVersion = "1.0.0";
    public static PluginInfo? PluginInfo { get; private set; }
    public new static ManualLogSource? Logger { get; private set; }

    private void Awake()
    {
        PluginInfo = Info;
        Logger = base.Logger;

        Assets.LoadAllAssetBundles();

        RegisterAllEmotes();
        
        EmoteRegistry.FinalizeRegistry();
    }

    private void RegisterAllEmotes()
    {
        EmoteRegistry.RegisterEmote<GoblinPainEmote>();
    }
}