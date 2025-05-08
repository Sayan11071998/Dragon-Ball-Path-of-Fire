using UnityEngine;
using System.Collections.Generic;

namespace DragonBall.Sound
{
    public class SoundService
    {
        private SoundScriptableObject soundConfig;
        private AudioSource primarySoundEffectsSource;
        private AudioSource backgroundMusicSource;
        private Queue<AudioSource> availableAudioSources;
        private Dictionary<SoundType, AudioSource> activeSoundEffects;

        public SoundService(
            SoundScriptableObject config,
            AudioSource sfxSource,
            AudioSource bgmSource,
            Queue<AudioSource> audioSourcesPool,
            Dictionary<SoundType, AudioSource> activeEffects)
        {
            soundConfig = config;
            primarySoundEffectsSource = sfxSource;
            backgroundMusicSource = bgmSource;
            availableAudioSources = audioSourcesPool;
            activeSoundEffects = activeEffects;
        }

        public void PlaySoundEffects(SoundType soundType, bool loop = false)
        {
            AudioClip clip = GetSoundClip(soundType);

            if (clip == null) return;

            if (activeSoundEffects.TryGetValue(soundType, out AudioSource existingSource))
            {
                if (existingSource.isPlaying && existingSource.loop && loop) return;
                StopSoundEffect(soundType);
            }

            AudioSource audioSource = GetAudioSource();

            if (audioSource == null)
            {
                Debug.LogWarning("No available audio sources to play sound effect: " + soundType);
                return;
            }

            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();

            activeSoundEffects[soundType] = audioSource;
        }

        public void PlayBackgroundMusic(SoundType soundType, bool loop = true)
        {
            AudioClip clip = GetSoundClip(soundType);

            if (clip != null)
            {
                backgroundMusicSource.loop = loop;
                backgroundMusicSource.clip = clip;
                backgroundMusicSource.Play();
            }
        }

        public void StopSoundEffect(SoundType soundType)
        {
            if (activeSoundEffects.TryGetValue(soundType, out AudioSource audioSource))
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                    audioSource.clip = null;
                }

                availableAudioSources.Enqueue(audioSource);
                activeSoundEffects.Remove(soundType);
            }
        }

        private AudioSource GetAudioSource()
        {
            if (availableAudioSources.Count == 0)
            {
                foreach (var kvp in activeSoundEffects)
                {
                    AudioSource source = kvp.Value;
                    if (!source.isPlaying)
                    {
                        SoundType soundType = kvp.Key;
                        activeSoundEffects.Remove(soundType);
                        return source;
                    }
                }
                return null;
            }

            return availableAudioSources.Dequeue();
        }

        private AudioClip GetSoundClip(SoundType soundType) => soundConfig.GetSoundClip(soundType);
    }
}