using System.Collections;
using UnityEngine;

namespace DragonBall.GameCamera
{
    public class CameraShakeService
    {
        private Camera mainCamera;
        private Transform cameraTransform;
        private MonoBehaviour coroutineRunner;

        private Vector3 originalPosition;
        private float shakeIntensity;
        private bool isShaking = false;

        public CameraShakeService(Camera camera, MonoBehaviour coroutineRunner)
        {
            mainCamera = camera;
            cameraTransform = camera.transform;
            this.coroutineRunner = coroutineRunner;
        }

        public void ShakeCamera(float intensity, float duration) => coroutineRunner.StartCoroutine(ShakeCameraCoroutine(intensity, duration));

        public Vector3 ApplyShake(Vector3 position)
        {
            if (!isShaking) return position;

            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
            Vector3 shakenPosition = position + shakeOffset;

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
    }
}