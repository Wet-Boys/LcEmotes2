using EmotesAPI;
using LethalEmotesAPI.ImportV2;
using System.Collections.Generic;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.JoinSpots;

public abstract class AbstractJoinSpot
{
    public virtual void OnSpotJoined(GameObject emoteSpot, BoneMapper joiner, BoneMapper host) { }
}