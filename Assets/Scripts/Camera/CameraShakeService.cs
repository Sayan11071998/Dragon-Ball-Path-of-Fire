using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

namespace DragonBall.Core
{
    public class CameraShakeService
    {
        private CinemachineStateDrivenCamera cinemachineCamera;
        private CinemachineBasicMultiChannelPerlin[] noiseComponents;
        private Coroutine shakeCoroutine;
        private MonoBehaviour coroutineRunner;

        public CameraShakeService(CinemachineStateDrivenCamera camera, MonoBehaviour runner)
        {
            cinemachineCamera = camera;
            coroutineRunner = runner;
            InitializeNoiseComponents();
        }

        private void InitializeNoiseComponents()
        {
            CinemachineCamera[] childCameras = cinemachineCamera.GetComponentsInChildren<CinemachineCamera>();
            noiseComponents = new CinemachineBasicMultiChannelPerlin[childCameras.Length];

            for (int i = 0; i < childCameras.Length; i++)
            {
                var noise = childCameras[i].GetComponent<CinemachineBasicMultiChannelPerlin>();
                if (noise == null)
                {
                    noise = childCameras[i].gameObject.AddComponent<CinemachineBasicMultiChannelPerlin>();
                    noise.NoiseProfile = Resources.Load<NoiseSettings>("CameraShakeNoiseProfile");
                }
                noiseComponents[i] = noise;
            }
        }

        public void ShakeCamera(float intensity, float duration)
        {
            if (shakeCoroutine != null)
                coroutineRunner.StopCoroutine(shakeCoroutine);
            shakeCoroutine = coroutineRunner.StartCoroutine(ShakeCameraCoroutine(intensity, duration));
        }

        public void ShakeCameraWithDecay(float intensity, float duration, float decreaseRate = 1.0f)
        {
            if (shakeCoroutine != null)
                coroutineRunner.StopCoroutine(shakeCoroutine);

            shakeCoroutine = coroutineRunner.StartCoroutine(ShakeCameraDecayCoroutine(intensity, duration, decreaseRate));
        }

        public void StopShake()
        {
            if (shakeCoroutine != null)
            {
                coroutineRunner.StopCoroutine(shakeCoroutine);
                SetShakeIntensity(0f);
            }
        }

        private IEnumerator ShakeCameraCoroutine(float intensity, float duration)
        {
            SetShakeIntensity(intensity);
            yield return new WaitForSeconds(duration);

            SetShakeIntensity(0f);
            shakeCoroutine = null;
        }

        private IEnumerator ShakeCameraDecayCoroutine(float intensity, float duration, float decreaseRate)
        {
            float initialIntensity = intensity;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float currentIntensity = Mathf.Lerp(initialIntensity, 0f, (elapsed / duration) * decreaseRate);
                SetShakeIntensity(currentIntensity);

                elapsed += Time.deltaTime;
                yield return null;
            }

            SetShakeIntensity(0f);
            shakeCoroutine = null;
        }

        private void SetShakeIntensity(float intensity)
        {
            foreach (var noise in noiseComponents)
            {
                if (noise != null)
                    noise.AmplitudeGain = intensity;
            }
        }
    }
}