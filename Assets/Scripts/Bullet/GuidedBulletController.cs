using UnityEngine;

namespace DragonBall.Bullet
{
    public class GuidedBulletController : BulletController
    {
        private GuidedBulletView guidedBulletView;
        private GuidedBulletModel guidedBulletModel;
        private Transform target;
        private bool isGuided = true;
        private float guidanceDelay;
        private float maxGuidanceTime;

        public GuidedBulletController(
            GuidedBulletModel _bulletModel,
            GuidedBulletView _bulletView,
            BulletPool _bulletPool)
            : base(_bulletModel, _bulletView, _bulletPool)
        {
            guidedBulletModel = _bulletModel;
            guidedBulletView = _bulletView;
            guidedBulletView.SetGuidedController(this);
        }

        public void Activate(Vector2 position, Vector2 direction, Transform target, BulletTargetType targetType = BulletTargetType.Player)
        {
            base.Activate(position, direction, targetType);
            this.target = target;
            isGuided = true;
            guidanceDelay = guidedBulletModel.GuidanceDelay;
            maxGuidanceTime = guidedBulletModel.MaxGuidanceTime;
        }

        public override void Update()
        {
            base.Update();

            float elapsedTime = Time.time - creationTime;

            // Only start guiding after the delay
            if (elapsedTime < guidanceDelay)
                return;

            // Stop guiding after max guidance time
            if (elapsedTime > maxGuidanceTime + guidanceDelay)
            {
                DisableGuiding();
                return;
            }

            if (target != null && isGuided)
            {
                Vector2 direction = (target.position - guidedBulletView.transform.position).normalized;
                Vector2 currentVelocity = guidedBulletView.GetVelocity();
                float currentSpeed = currentVelocity.magnitude;

                // Calculate new velocity based on rotation speed
                Vector2 newVelocity = Vector2.Lerp(
                    currentVelocity.normalized,
                    direction,
                    guidedBulletModel.RotationSpeed * Time.deltaTime / 100f
                ).normalized * currentSpeed;

                guidedBulletView.SetVelocity(newVelocity);

                // Update rotation to match velocity direction
                float angle = Mathf.Atan2(newVelocity.y, newVelocity.x) * Mathf.Rad2Deg;
                guidedBulletView.SetRotation(angle);
            }
        }

        public void DisableGuiding() => isGuided = false;
    }
}