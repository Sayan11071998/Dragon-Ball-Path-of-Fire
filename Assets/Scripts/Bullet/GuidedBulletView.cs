using UnityEngine;

namespace DragonBall.Bullet
{
    public class GuidedBulletView : BulletView
    {
        [SerializeField] private float trailLength = 0.5f;
        [SerializeField] private Color trailColor = Color.red;

        private GuidedBulletController guidedController;
        private TrailRenderer trailRenderer;

        public void SetGuidedController(GuidedBulletController controllerToSet) => guidedController = controllerToSet;

        protected override void Awake()
        {
            base.Awake();
            SetupTrailRenderer();
        }

        protected override void Update()
        {
            if (guidedController != null)
                guidedController.Update();
        }

        private void SetupTrailRenderer()
        {
            if (!TryGetComponent<TrailRenderer>(out trailRenderer))
            {
                trailRenderer = gameObject.AddComponent<TrailRenderer>();
                trailRenderer.time = trailLength;
                trailRenderer.startColor = trailColor;
                trailRenderer.endColor = new Color(trailColor.r, trailColor.g, trailColor.b, 0f);
                trailRenderer.startWidth = 0.1f;
                trailRenderer.endWidth = 0.05f;
            }
        }

        public Vector2 GetVelocity() => rb.linearVelocity;

        public void SetRotation(float angle) => transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}