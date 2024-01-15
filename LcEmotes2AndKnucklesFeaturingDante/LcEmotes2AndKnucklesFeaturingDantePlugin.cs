using BepInEx;
using EmotesAPI;
using UnityEngine;
using UnityEngine.Playables;

namespace LcEmotes2AndKnucklesFeaturingDante;


[BepInDependency("com.weliveinasociety.CustomEmotesAPI")]
[BepInPlugin(ModGuid, ModName, ModVersion)]
public class LcEmotes2AndKnucklesFeaturingDantePlugin : BaseUnityPlugin
{
    public const string ModGuid = "com.gemumoddo.lc_emotes2_and_knuckles_featuring_dante";
    public const string ModName = "Emotes 2 And Knuckles Featuring Dante";
    public const string ModVersion = "1.0.0";
    public static PluginInfo PInfo { get; private set; }

    private void Awake()
    {
        PInfo = Info;
        Assets.LoadAssetBundlesFromFolder("assetbundles");
        AnimationClipParams param = new AnimationClipParams();
        param.animationClip = [Assets.Load<AnimationClip>("emotes/goblinpain/scene.anim")];
        param.looping = true;
        param.syncAnim = true;
        param.syncAudio = true;
        param.customName = "Pain";
        param.lockType = AnimationClipParams.LockType.headBobbing;
        param.willGetClaimedByDMCA = false;
        param.thirdPerson = true;

        CustomEmotesAPI.AddCustomAnimation(param);

        CustomEmotesAPI.animChanged += CustomEmotesAPI_animChanged;
    }

    private void CustomEmotesAPI_animChanged(string newAnimation, BoneMapper mapper)
    {
        int prop1;
        switch (newAnimation)
        {
            case "Scene":
                prop1 = mapper.props.Count;
                mapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>("emotes/goblinpain/painholder.prefab")));
                mapper.props[prop1].GetComponentInChildren<PlayableDirector>().time = 0;
                mapper.props[prop1].GetComponentInChildren<PlayableDirector>().Play();
                mapper.props[prop1].transform.SetParent(mapper.mapperBody.transform);
                mapper.props[prop1].transform.localEulerAngles = Vector3.zero;
                mapper.props[prop1].transform.localPosition = new Vector3(0,1.5f,0);
                break;
            default:
                break;
        }
    }
}