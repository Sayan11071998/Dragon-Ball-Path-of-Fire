using UnityEngine;
using System.Collections;
using DragonBall.Core;
using DragonBall.Player.PlayerMVC;
using DragonBall.GameStrings;

namespace DragonBall.GameCamera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Follow Settings")]
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);

        [Header("Camera Size Settings")]
        [SerializeField] private float defaultOrthographicSize = 6f;
        [SerializeField] private float runOrthographicSize = 8f;
        [SerializeField] private float jumpOrthographicSize = 8f;
        [SerializeField] private float flyOrthographicSize = 10f;
        [SerializeField] private float superSaiyanOrthographicSize = 10f;

        [Header("Transition Settings")]
        [SerializeField] private float defaultTransitionSpeed = 0.1f;
        [SerializeField] private float runTransitionSpeed = 1.2f;
        [SerializeField] private float jumpTransitionSpeed = 2f;
        [SerializeField] private float flyTransitionSpeed = 1.5f;
        [SerializeField] private float superSaiyanTransitionSpeed = 1f;
        [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Transform playerTransform;
        private Camera mainCamera;
        private bool playerFound = false;
        private PlayerView playerView;

        private float targetOrthographicSize;
        private float currentVelocity;
        private float currentTransitionSpeed;

        private void Awake()
        {
            mainCamera = GetComponent<Camera>();

            if (mainCamera != null && mainCamera.orthographic)
            {
                mainCamera.orthographicSize = defaultOrthographicSize;
                targetOrthographicSize = defaultOrthographicSize;
                currentTransitionSpeed = defaultTransitionSpeed;
            }
        }

        private void Start() => StartCoroutine(FindPlayerRoutine());

        private IEnumerator FindPlayerRoutine()
        {
            while (!playerFound)
            {
                GameObject player = GameObject.Find(GameString.PlayerPrefabName);

                if (player != null)
                {
                    playerTransform = player.transform;
                    playerView = player.GetComponent<PlayerView>();
                    playerFound = true;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void Update()
        {
            if (!playerFound || playerView == null) return;

            UpdateTargetOrthographicSize();

            if (mainCamera != null && mainCamera.orthographic)
            {
                mainCamera.orthographicSize = Mathf.SmoothDamp(
                    mainCamera.orthographicSize,
                    targetOrthographicSize,
                    ref currentVelocity,
                    1 / currentTransitionSpeed
                );
            }
        }

        private void UpdateTargetOrthographicSize()
        {
            float newTargetSize = defaultOrthographicSize;
            float newTransitionSpeed = defaultTransitionSpeed;

            if (playerView.IsSuperSaiyan)
            {
                newTargetSize = superSaiyanOrthographicSize;
                newTransitionSpeed = superSaiyanTransitionSpeed;
            }
            else if (playerView.Animator.GetBool(GameString.PlayerAnimationFlightBool))
            {
                newTargetSize = flyOrthographicSize;
                newTransitionSpeed = flyTransitionSpeed;
            }
            else if (playerView.Animator.GetBool(GameString.PlayerAnimationJumpBool))
            {
                newTargetSize = jumpOrthographicSize;
                newTransitionSpeed = jumpTransitionSpeed;
            }
            else if (playerView.Animator.GetBool(GameString.PlayerAnimationRunBool))
            {
                newTargetSize = runOrthographicSize;
                newTransitionSpeed = runTransitionSpeed;
            }

            targetOrthographicSize = newTargetSize;
            currentTransitionSpeed = newTransitionSpeed;
        }

        private void LateUpdate()
        {
            if (!playerFound || playerTransform == null) return;

            Vector3 desiredPosition = playerTransform.position + offset;

            if (GameService.Instance != null && GameService.Instance.cameraShakeService != null)
                desiredPosition = GameService.Instance.cameraShakeService.ApplyShake(desiredPosition);

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

        public void SetOrthographicSize(float newSize, float transitionDuration = 0f) => StartCoroutine(TransitionOrthographicSize(newSize, transitionDuration));

        private IEnumerator TransitionOrthographicSize(float newSize, float duration)
        {
            if (duration <= 0f || mainCamera == null || !mainCamera.orthographic)
            {
                targetOrthographicSize = newSize;
                yield break;
            }

            float startSize = mainCamera.orthographicSize;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                float curvedT = transitionCurve.Evaluate(t);

                targetOrthographicSize = Mathf.Lerp(startSize, newSize, curvedT);
                yield return null;
            }

            targetOrthographicSize = newSize;
        }
    }
}