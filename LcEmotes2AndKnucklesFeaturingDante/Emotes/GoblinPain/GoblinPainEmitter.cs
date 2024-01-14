using System;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain;

public class GoblinPainEmitter : MonoBehaviour
{
    public ParticleSystem[] goblinParticleEmitter = [];
    public float timeBetweenPulse = 0.25f;

    private int _pulseIndex;
    private float _timer;

    private void Update()
    {
        _timer -= Time.unscaledDeltaTime;
        
        if (_timer > 0)
            return;
        
        _timer = timeBetweenPulse;
        goblinParticleEmitter[_pulseIndex].Play();

        _pulseIndex++;
        if (_pulseIndex >= goblinParticleEmitter.Length)
            _pulseIndex = 0;
    }
}