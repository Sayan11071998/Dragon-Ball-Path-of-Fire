using DragonBall.Bullet.BulletData;
using DragonBall.Bullet.BulletUtilities;
using DragonBall.Bullet.ParentMVC;
using UnityEngine;

namespace DragonBall.Bullet.GuidedBulletMVC
{
    public class GuidedBulletController : BulletController
    {
        private GuidedBulletView guidedBulletView;
        private GuidedBulletModel guidedBulletModel;
        private Transform target;
        private Vector3 targetOffset = new Vector3(0f, 0.75f, 0f);

        private float guidanceDelay;
        private float maxGuidanceTime;

        private bool isGuided = true;

        public GuidedBulletController(GuidedBulletModel _bulletModel, GuidedBulletView _bulletView, BulletPool _bulletPool)
            : base(_bulletModel, _bulletView, _bulletPool)
        {
            guidedBulletModel = _bulletModel;
            guidedBulletView = _bulletView;
            guidedBulletView.SetGuidedController(this);
        }

        public void Activate(Vector2 position, Vector2 direction, Transform targetToSet, BulletTargetType targetType = BulletTargetType.Player)
        {
            base.Activate(position, direction, targetType);
            target = targetToSet;
            isGuided = true;
            guidanceDelay = guidedBulletModel.GuidanceDelay;
            maxGuidanceTime = guidedBulletModel.MaxGuidanceTime;

            float initialAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            guidedBulletView.SetRotation(initialAngle);
        }

        public override void Update()
        {
            base.Update();

            float elapsedTime = Time.time - creationTime;

            if (elapsedTime < guidanceDelay)
                return;

            if (elapsedTime > maxGuidanceTime + guidanceDelay)
            {
                DisableGuiding();
                return;
            }

            if (target != null && isGuided)
            {
                Vector3 targetPosition = target.position + targetOffset;
                Vector2 targetPosition2D = new Vector2(targetPosition.x, targetPosition.y);
                Vector2 currentPosition2D = new Vector2(guidedBulletView.transform.position.x, guidedBulletView.transform.position.y);
                Vector2 direction = (targetPosition2D - currentPosition2D).normalized;
                Vector2 currentVelocity = guidedBulletView.GetVelocity();
                float currentSpeed = currentVelocity.magnitude;

                Vector2 newVelocity = Vector2.Lerp(
                    currentVelocity.normalized,
                    direction,
                    guidedBulletModel.RotationSpeed * Time.deltaTime / 100f
                ).normalized * currentSpeed;

                guidedBulletView.SetVelocity(newVelocity);

                float angle = Mathf.Atan2(newVelocity.y, newVelocity.x) * Mathf.Rad2Deg;
                guidedBulletView.SetRotation(angle);
            }
        }

        public void DisableGuiding() => isGuided = false;

        public void SetTargetOffset(Vector3 offset) => targetOffset = offset;
    }
}