using UnityEngine;
using System.Collections;
using DragonBall.Core;

namespace DragonBall.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Follow Settings")]
        [SerializeField] private float followSpeed = 5f;
        [SerializeField] private Vector2 offset = new Vector2(0f, 1f);
        [SerializeField] private bool enableSmoothing = true;

        [Header("Bounds")]
        [SerializeField] private bool enableBoundaries = false;
        [SerializeField] private float minX = -10f;
        [SerializeField] private float maxX = 10f;
        [SerializeField] private float minY = -5f;
        [SerializeField] private float maxY = 5f;

        [Header("Zoom Settings")]
        [SerializeField] private float defaultOrthoSize = 5f;
        [SerializeField] private float zoomTransitionSpeed = 2f;

        private Transform target;
        private UnityEngine.Camera mainCamera;
        private float currentOrthoSize;
        private float targetOrthoSize;
        private Vector3 currentVelocity;
        private bool isTransitioning = false;

        public enum CameraState
        {
            Normal,
            SuperSaiyan,
            BossFight,
            Cinematic
        }

        [System.Serializable]
        public class CameraStateSettings
        {
            public CameraState state;
            public float orthoSize = 5f;
            public Vector2 offsetOverride = new Vector2(0f, 0f);
            public bool useOffsetOverride = false;
        }

        [Header("Camera States")]
        [SerializeField] private CameraStateSettings[] cameraStates;
        private CameraState currentState = CameraState.Normal;

        private void Awake()
        {
            mainCamera = GetComponent<UnityEngine.Camera>();
            currentOrthoSize = mainCamera.orthographicSize;
            targetOrthoSize = currentOrthoSize;
        }

        private void Start()
        {
            StartCoroutine(FindPlayerCoroutine());
        }

        private IEnumerator FindPlayerCoroutine()
        {
            yield return new WaitUntil(() => GameService.Instance != null &&
                                            GameService.Instance.playerService != null &&
                                            GameService.Instance.playerService.PlayerController != null);

            // Set the target to the player
            target = GameService.Instance.playerService.PlayerController.PlayerView.transform;

            Debug.Log("Camera connected to player");
        }

        private void LateUpdate()
        {
            if (target == null) return;

            UpdateZoom();
            FollowTarget();
        }

        private void FollowTarget()
        {
            Vector3 targetPosition = target.position;
            Vector2 currentOffset = offset;

            // Check if current state has an offset override
            foreach (var state in cameraStates)
            {
                if (state.state == currentState && state.useOffsetOverride)
                {
                    currentOffset = state.offsetOverride;
                    break;
                }
            }

            // Apply offset
            targetPosition.x += currentOffset.x;
            targetPosition.y += currentOffset.y;
            targetPosition.z = transform.position.z; // Maintain camera z position

            // Apply boundaries if enabled
            if (enableBoundaries)
            {
                targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
            }

            // Apply smoothing
            if (enableSmoothing)
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f / followSpeed);
            }
            else
            {
                transform.position = targetPosition;
            }
        }

        private void UpdateZoom()
        {
            if (Mathf.Approximately(mainCamera.orthographicSize, targetOrthoSize)) return;

            // Smoothly interpolate to target ortho size
            mainCamera.orthographicSize = Mathf.Lerp(
                mainCamera.orthographicSize,
                targetOrthoSize,
                Time.deltaTime * zoomTransitionSpeed
            );
        }

        public void SetCameraState(CameraState newState, float transitionTime = 0.5f)
        {
            currentState = newState;

            // Find the settings for this state
            foreach (var state in cameraStates)
            {
                if (state.state == newState)
                {
                    targetOrthoSize = state.orthoSize;
                    break;
                }
            }

            Debug.Log($"Camera state changed to {newState} with ortho size {targetOrthoSize}");
        }

        public void SetCustomOrthoSize(float size, float transitionTime = 0.5f)
        {
            targetOrthoSize = size;
        }

        public void ShakeCamera(float intensity, float duration)
        {
            StartCoroutine(ShakeCameraCoroutine(intensity, duration));
        }

        public void ShakeCameraWithDecay(float intensity, float duration, float decreaseRate = 1.0f)
        {
            StartCoroutine(ShakeCameraDecayCoroutine(intensity, duration, decreaseRate));
        }

        private IEnumerator ShakeCameraCoroutine(float intensity, float duration)
        {
            Vector3 originalPosition = transform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * intensity;
                float y = Random.Range(-1f, 1f) * intensity;

                transform.position = new Vector3(
                    originalPosition.x + x,
                    originalPosition.y + y,
                    originalPosition.z
                );

                elapsed += Time.deltaTime;
                originalPosition = Vector3.Lerp(originalPosition, target.position, Time.deltaTime * followSpeed);

                yield return null;
            }

            transform.position = originalPosition;
        }

        private IEnumerator ShakeCameraDecayCoroutine(float intensity, float duration, float decreaseRate)
        {
            Vector3 originalPosition = transform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float currentIntensity = Mathf.Lerp(intensity, 0f, (elapsed / duration) * decreaseRate);
                float x = Random.Range(-1f, 1f) * currentIntensity;
                float y = Random.Range(-1f, 1f) * currentIntensity;

                transform.position = new Vector3(
                    originalPosition.x + x,
                    originalPosition.y + y,
                    originalPosition.z
                );

                elapsed += Time.deltaTime;
                originalPosition = Vector3.Lerp(originalPosition, target.position, Time.deltaTime * followSpeed);

                yield return null;
            }

            transform.position = originalPosition;
        }
    }
}