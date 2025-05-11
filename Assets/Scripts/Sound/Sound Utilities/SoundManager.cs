using UnityEngine;
using System.Collections.Generic;
using DragonBall.Utilities;
using DragonBall.Sound.SoundData;

namespace DragonBall.Sound.SoundUtilities
{
    public class SoundManager : GenericMonoSingleton<SoundManager>
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource soundEffectsSource;
        [SerializeField] private AudioSource backgroundMusicSource;
        [SerializeField] private int additionalAudioSourcesCount = 5;

        [Header("Sound Configuration")]
        [SerializeField] private SoundScriptableObject soundScriptableObject;

        private SoundService soundService;
        private Queue<AudioSource> availableAudioSources = new Queue<AudioSource>();
        private Dictionary<SoundType, AudioSource> activeSoundEffects = new Dictionary<SoundType, AudioSource>();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            CreateAdditionalAudioSources();
            soundService = new SoundService(soundScriptableObject, soundEffectsSource, backgroundMusicSource, availableAudioSources, activeSoundEffects);
        }

        private void CreateAdditionalAudioSources()
        {
            availableAudioSources.Enqueue(soundEffectsSource);

            for (int i = 0; i < additionalAudioSourcesCount; i++)
            {
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.playOnAwake = false;
                newSource.volume = soundEffectsSource.volume;
                newSource.spatialBlend = soundEffectsSource.spatialBlend;
                newSource.outputAudioMixerGroup = soundEffectsSource.outputAudioMixerGroup;
                availableAudioSources.Enqueue(newSource);
            }
        }

        public void PlaySoundEffect(SoundType soundType, bool loop = false) => soundService.PlaySoundEffects(soundType, loop);

        public void PlayBackgroundMusic(SoundType soundType, bool loop = true) => soundService.PlayBackgroundMusic(soundType, loop);

        public void StopSoundEffect(SoundType soundType) => soundService.StopSoundEffect(soundType);
    }
}