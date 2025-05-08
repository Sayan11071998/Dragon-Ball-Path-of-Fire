using UnityEngine;

namespace DragonBall.Sound
{
    [CreateAssetMenu(fileName = "SoundScriptableObject", menuName = "Sound/SoundScriptableObject")]
    public class SoundScriptableObject : ScriptableObject
    {
        public Sounds[] audioList;
    }
}