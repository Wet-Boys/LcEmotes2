using LcEmotes2AndKnucklesFeaturingDante.Common.Rand;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Data;

[CreateAssetMenu(fileName = "TwitchUsernameList", menuName = "Emotes 2/Twitch/Username List")]
public class TwitchUsernameList : WeightedRandomData<TwitchUsernameEntry>;