using UnityEngine;
using System.Collections.Generic;

namespace DragonBall.Sound.SoundData
{
    [CreateAssetMenu(fileName = "SoundConfig", menuName = "Sound/SoundConfig")]
    public class SoundScriptableObject : ScriptableObject
    {
        [System.Serializable]
        public class SoundEntry
        {
            public SoundType soundType;
            public AudioClip audioClip;
        }

        [SerializeField] private List<SoundEntry> soundEntries = new List<SoundEntry>();

        public AudioClip GetSoundClip(SoundType soundType)
        {
            foreach (var entry in soundEntries)
            {
                if (entry.soundType == soundType)
                    return entry.audioClip;
            }

            Debug.LogWarning($"Sound clip for {soundType} not found!");
            return null;
        }
    }
}