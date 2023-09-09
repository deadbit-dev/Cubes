using System;
using UnityEngine;

namespace JoyTeam.Game
{
    public class ParticleSystemStopped : MonoBehaviour
    {
        public event Action OnStop = delegate {};
        public void OnParticleSystemStopped() => OnStop();
    }
}
