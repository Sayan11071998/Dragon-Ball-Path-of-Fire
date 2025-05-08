using UnityEngine;
using DragonBall.Utilities;

namespace DragonBall.Sound
{
    public class SoundManager : GenericMonoSingleton<SoundManager>
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource soundEffectsSource;
        [SerializeField] private AudioSource backgroundMusicSource;

        [Header("Sound Configuration")]
        [SerializeField] private SoundScriptableObject soundScriptableObject;

        private SoundService soundService;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            soundService = new SoundService(soundScriptableObject, soundEffectsSource, backgroundMusicSource);
        }

        public void PlaySoundEffect(SoundType soundType, bool loop = false) => soundService.PlaySoundEffects(soundType, loop);

        public void PlayBackgroundMusic(SoundType soundType, bool loop = true) => soundService.PlayBackgroundMusic(soundType, loop);
    }
}