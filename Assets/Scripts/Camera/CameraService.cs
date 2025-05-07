using UnityEngine;
using DragonBall.Player;

namespace DragonBall.Camera
{
    public class CameraService
    {
        private UnityEngine.Camera mainCamera;
        private CameraController cameraController;
        private Transform cameraTransform;

        public UnityEngine.Camera MainCamera => mainCamera;
        public CameraController CameraController => cameraController;
        public Transform CameraTransform => cameraTransform;

        public CameraService(UnityEngine.Camera cameraToUse = null, CameraController cameraControllerPrefab = null)
        {
            // If no camera was passed, find the main camera
            if (cameraToUse == null)
            {
                mainCamera = UnityEngine.Camera.main;

                if (mainCamera == null)
                {
                    Debug.LogError("No main camera found in the scene. Please tag a camera as MainCamera.");
                    mainCamera = new GameObject("Main Camera").AddComponent<UnityEngine.Camera>();
                    mainCamera.tag = "MainCamera";
                }
            }
            else
            {
                mainCamera = cameraToUse;
            }

            cameraTransform = mainCamera.transform;

            // Add or get CameraController
            cameraController = mainCamera.gameObject.GetComponent<CameraController>();
            if (cameraController == null)
            {
                if (cameraControllerPrefab != null)
                {
                    // Instantiate from prefab if provided
                    CameraController prefabInstance = Object.Instantiate(cameraControllerPrefab);
                    prefabInstance.transform.SetParent(mainCamera.transform);
                    cameraController = prefabInstance;
                }
                else
                {
                    // Otherwise, add component directly
                    cameraController = mainCamera.gameObject.AddComponent<CameraController>();
                }
            }

            Debug.Log("Camera service initialized");
        }

        public void ShakeCamera(float intensity, float duration)
        {
            cameraController.ShakeCamera(intensity, duration);
        }

        public void ShakeCameraWithDecay(float intensity, float duration, float decreaseRate = 1.0f)
        {
            cameraController.ShakeCameraWithDecay(intensity, duration, decreaseRate);
        }

        public void SetCameraState(CameraController.CameraState state, float transitionTime = 0.5f)
        {
            cameraController.SetCameraState(state, transitionTime);
        }

        public void SetOrthoSize(float size, float transitionTime = 0.5f)
        {
            cameraController.SetCustomOrthoSize(size, transitionTime);
        }

        public void HandlePlayerStateChange(PlayerState playerState)
        {
            switch (playerState)
            {
                case PlayerState.NORMAL:
                    SetCameraState(CameraController.CameraState.Normal);
                    break;

                case PlayerState.SUPER_SAIYAN:
                    SetCameraState(CameraController.CameraState.SuperSaiyan);
                    break;
            }
        }
    }
}