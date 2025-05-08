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

        public void StopBackgroundMusic() => backgroundMusicSource.Stop();

        public void SetBackgroundMusicVolume(float volume) => backgroundMusicSource.volume = Mathf.Clamp01(volume);

        public void SetSoundEffectsVolume(float volume) => soundEffectsSource.volume = Mathf.Clamp01(volume);
    }
}