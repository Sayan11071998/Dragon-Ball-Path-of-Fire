using UnityEngine;
using System.Collections.Generic;

namespace DragonBall.Sound.SoundData
{
    [CreateAssetMenu(fileName = "SoundScriptableObject", menuName = "Sound/SoundScriptableObject")]
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

            return null;
        }
    }
}