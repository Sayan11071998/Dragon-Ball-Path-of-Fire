using System;
using UnityEngine;

namespace DragonBall.Sound.SoundData
{
    [Serializable]
    public struct Sounds
    {
        public SoundType soundType;
        public AudioClip audio;
    }
}