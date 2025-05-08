using System.Collections;
using UnityEngine;
using DragonBall.GameStrings;

namespace DragonBall.Core
{
    public class CameraController : MonoBehaviour
    {
        [Header("Follow Settings")]
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
        [SerializeField] private float orthographicSize = 8f;

        private Transform playerTransform;
        private Camera mainCamera;
        private bool playerFound = false;

        private void Awake()
        {
            mainCamera = GetComponent<Camera>();

            if (mainCamera != null && mainCamera.orthographic)
                mainCamera.orthographicSize = orthographicSize;
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
                    playerFound = true;
                    Debug.Log("Player found for camera to follow!");
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void LateUpdate()
        {
            if (!playerFound || playerTransform == null) return;

            Vector3 desiredPosition = playerTransform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

        public void SetOrthographicSize(float newSize)
        {
            orthographicSize = newSize;
            if (mainCamera != null && mainCamera.orthographic)
                mainCamera.orthographicSize = orthographicSize;
        }
    }
}