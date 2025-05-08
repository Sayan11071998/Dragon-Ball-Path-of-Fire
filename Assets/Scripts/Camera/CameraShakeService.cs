using System.Collections;
using UnityEngine;

namespace DragonBall.Core
{
    public class CameraShakeService
    {
        private Camera mainCamera;
        private Transform cameraTransform;
        private MonoBehaviour coroutineRunner;

        [Header("Camera Shake Settings")]
        private float shakeDecay = 0.8f;

        private Vector3 originalPosition;
        private float shakeIntensity;
        private bool isShaking = false;

        public CameraShakeService(Camera camera, MonoBehaviour coroutineRunner, float shakeDecay = 0.8f)
        {
            this.mainCamera = camera;
            this.cameraTransform = camera.transform;
            this.coroutineRunner = coroutineRunner;
            this.shakeDecay = shakeDecay;
        }

        public void ShakeCamera(float intensity, float duration)
        {
            coroutineRunner.StartCoroutine(ShakeCameraCoroutine(intensity, duration));
        }

        public void ShakeCameraWithDecay(float intensity, float duration)
        {
            shakeIntensity = intensity;
            isShaking = true;

            coroutineRunner.StartCoroutine(StopShakeAfterDuration(duration));
        }

        public bool IsShaking => isShaking;

        public Vector3 ApplyShake(Vector3 position)
        {
            if (!isShaking) return position;

            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
            Vector3 shakenPosition = position + shakeOffset;

            shakeIntensity *= shakeDecay;

            if (shakeIntensity < 0.01f)
            {
                isShaking = false;
                shakeIntensity = 0f;
            }

            return shakenPosition;
        }

        private IEnumerator ShakeCameraCoroutine(float intensity, float duration)
        {
            originalPosition = cameraTransform.position;
            float elapsed = 0f;
            isShaking = true;

            while (elapsed < duration)
            {
                Vector3 shakeOffset = Random.insideUnitSphere * intensity;
                cameraTransform.position = originalPosition + shakeOffset;

                elapsed += Time.deltaTime;
                yield return null;
            }

            isShaking = false;
        }

        private IEnumerator StopShakeAfterDuration(float duration)
        {
            yield return new WaitForSeconds(duration);
            isShaking = false;
            shakeIntensity = 0f;
        }
    }
}