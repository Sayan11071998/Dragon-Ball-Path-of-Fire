using UnityEngine;
using System;
using DragonBall.Events;

namespace DragonBall.Sound
{
    public class SoundService
    {
        private SoundScriptableObject soundScriptableObject;
        private AudioSource audioEffects;
        private AudioSource backgroundMusic;

        public SoundService(SoundScriptableObject soundScriptableObject, AudioSource audioEffectSource, AudioSource bgMusicSource)
        {
            this.soundScriptableObject = soundScriptableObject;
            audioEffects = audioEffectSource;
            backgroundMusic = bgMusicSource;

            // Don't auto-play background music here, leave that to the caller
            RegisterSoundEventListeners();
        }

        private void RegisterSoundEventListeners()
        {
            if (EventService.Instance == null) return;

            // Add your event listeners here
            // Example:
            // EventService.Instance.OnPlayerAttack.AddListener(PlayAttackSound);
        }

        public void UnregisterSoundEventListeners()
        {
            if (EventService.Instance == null) return;

            // Remove your event listeners here
            // Example:
            // EventService.Instance.OnPlayerAttack.RemoveListener(PlayAttackSound);
        }

        public void PlaySoundEffects(SoundType soundType, bool loopSound = false)
        {
            AudioClip clip = GetSoundClip(soundType);

            if (clip != null)
                audioEffects.PlayOneShot(clip);
            else
                Debug.LogWarning($"Audio clip not found for sound type: {soundType}");
        }

        public void PlayBackgroundMusic(SoundType soundType, bool loopSound = true)
        {
            AudioClip clip = GetSoundClip(soundType);
            if (clip != null)
            {
                if (backgroundMusic.clip != clip || !backgroundMusic.isPlaying)
                {
                    backgroundMusic.Stop();
                    backgroundMusic.loop = loopSound;
                    backgroundMusic.clip = clip;
                    backgroundMusic.Play();
                }
            }
            else
                Debug.LogWarning($"Audio clip not found for sound type: {soundType}");
        }

        private AudioClip GetSoundClip(SoundType soundType)
        {
            Sounds sound = Array.Find(soundScriptableObject.audioList, item => item.soundType == soundType);
            if (sound.audio == null)
                return null;

            return sound.audio;
        }
    }
}