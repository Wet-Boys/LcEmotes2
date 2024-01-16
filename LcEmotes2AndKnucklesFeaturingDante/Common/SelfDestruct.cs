using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Common;

public class SelfDestruct : MonoBehaviour
{
    public float timeUntilDestruction = 5f;

    private void OnEnable()
    {
        Destroy(gameObject, timeUntilDestruction);
    }
}