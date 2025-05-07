using UnityEngine;

namespace DragonBall.Camera
{
    public class ParallaxController : MonoBehaviour
    {
        [SerializeField] private float parallaxEffect = 0.5f;
        [SerializeField] private bool infiniteHorizontal = true;
        [SerializeField] private bool infiniteVertical = false;

        private Transform cameraTransform;
        private Vector3 lastCameraPosition;
        private float spriteWidth;
        private float spriteHeight;
        private float startPositionX;
        private float startPositionY;

        private void Start()
        {
            cameraTransform = UnityEngine.Camera.main.transform;
            lastCameraPosition = cameraTransform.position;

            // Get sprite dimensions
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteWidth = spriteRenderer.bounds.size.x;
                spriteHeight = spriteRenderer.bounds.size.y;
            }
            else
            {
                Debug.LogWarning("ParallaxController attached to object without SpriteRenderer", this);
                spriteWidth = 1f;
                spriteHeight = 1f;
            }

            startPositionX = transform.position.x;
            startPositionY = transform.position.y;
        }

        private void LateUpdate()
        {
            if (cameraTransform == null) return;

            // Calculate parallax
            Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

            // Apply the parallax effect based on distance from camera (background vs foreground)
            float parallaxX = (lastCameraPosition.x - cameraTransform.position.x) * parallaxEffect;
            float parallaxY = (lastCameraPosition.y - cameraTransform.position.y) * parallaxEffect;

            // Update position with parallax
            float newPositionX = transform.position.x + parallaxX;
            float newPositionY = transform.position.y + parallaxY;

            // Handle infinite scrolling
            if (infiniteHorizontal)
            {
                float relativeDistanceX = cameraTransform.position.x * (1 - parallaxEffect);

                if (relativeDistanceX > startPositionX + spriteWidth)
                {
                    startPositionX += spriteWidth;
                }
                else if (relativeDistanceX < startPositionX - spriteWidth)
                {
                    startPositionX -= spriteWidth;
                }

                newPositionX = startPositionX + (cameraTransform.position.x * parallaxEffect);
            }

            if (infiniteVertical)
            {
                float relativeDistanceY = cameraTransform.position.y * (1 - parallaxEffect);

                if (relativeDistanceY > startPositionY + spriteHeight)
                {
                    startPositionY += spriteHeight;
                }
                else if (relativeDistanceY < startPositionY - spriteHeight)
                {
                    startPositionY -= spriteHeight;
                }

                newPositionY = startPositionY + (cameraTransform.position.y * parallaxEffect);
            }

            // Update the position
            transform.position = new Vector3(newPositionX, newPositionY, transform.position.z);

            // Update last position
            lastCameraPosition = cameraTransform.position;
        }
    }
}