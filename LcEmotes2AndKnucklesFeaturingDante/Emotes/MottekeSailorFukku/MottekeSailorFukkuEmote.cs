
using EmotesAPI;
using LethalEmotesAPI.ImportV2;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.MottekeSailorFukku;

public class MottekeSailorFukkuEmote : AbstractEmote
{
    public override string AnimationClipName => "luckystarDance1";

    public override CustomEmoteParams GetClipParams()
    {
        return GetLuckyStarEmote(AnimationClipName);
    }
    internal static CustomEmoteParams GetLuckyStarEmote(string animClipName)
    {
        return new CustomEmoteParams
        {
            primaryAnimationClips = [Assets.Load<AnimationClip>($"Emotes/Lucky Star/FinalDances/{animClipName}.anim")],
            audioLoops = false,
            syncAnim = true,
            syncAudio = true,
            displayName = "Motteke! Sailor Fukku",
            lockType = AnimationClipParams.LockType.lockHead,
            willGetClaimedByDMCA = true,
            thirdPerson = true,
            preventMovement = true,
            allowJoining = false,
            visible = false,
        };
    }
}
public class MottekeSailorFukkuEmote2 : AbstractEmote
{
    public override string AnimationClipName => "luckystarDance2";
    public override CustomEmoteParams GetClipParams()
    {
        return MottekeSailorFukkuEmote.GetLuckyStarEmote(AnimationClipName);
    }
}
public class MottekeSailorFukkuEmote3 : AbstractEmote
{
    public override string AnimationClipName => "luckystarDance3";
    public override CustomEmoteParams GetClipParams()
    {
        return MottekeSailorFukkuEmote.GetLuckyStarEmote(AnimationClipName);
    }
}
public class MottekeSailorFukkuEmote4 : AbstractEmote
{
    public override string AnimationClipName => "luckystarDance4";
    public override CustomEmoteParams GetClipParams()
    {
        return MottekeSailorFukkuEmote.GetLuckyStarEmote(AnimationClipName);
    }
}
public class MottekeSailorFukkuEmote5 : AbstractEmote
{
    public override string AnimationClipName => "luckystarDance5";
    public override CustomEmoteParams GetClipParams()
    {
        return MottekeSailorFukkuEmote.GetLuckyStarEmote(AnimationClipName);
    }
}
public class MottekeSailorFukkuEmote6 : AbstractEmote
{
    public override string AnimationClipName => "luckystarDance6";
    public override CustomEmoteParams GetClipParams()
    {
        return MottekeSailorFukkuEmote.GetLuckyStarEmote(AnimationClipName);
    }
}
public class MottekeSailorFukkuEmote7 : AbstractEmote
{
    public override string AnimationClipName => "luckystarDance7";
    public override CustomEmoteParams GetClipParams()
    {
        return MottekeSailorFukkuEmote.GetLuckyStarEmote(AnimationClipName);
    }
}
public class MottekeSailorFukkuEmote8 : AbstractEmote
{
    public override string AnimationClipName => "luckystarDance8";
    public override CustomEmoteParams GetClipParams()
    {
        return MottekeSailorFukkuEmote.GetLuckyStarEmote(AnimationClipName);
    }
}
public class MottekeSailorFukkuEmote9 : AbstractEmote
{
    public override string AnimationClipName => "luckystarDance9";
    public override CustomEmoteParams GetClipParams()
    {
        return MottekeSailorFukkuEmote.GetLuckyStarEmote(AnimationClipName);
    }
}
public class MottekeSailorFukkuEmote10 : AbstractEmote
{
    public override string AnimationClipName => "luckystarDance10";
    public override CustomEmoteParams GetClipParams()
    {
        return MottekeSailorFukkuEmote.GetLuckyStarEmote(AnimationClipName);
    }
}