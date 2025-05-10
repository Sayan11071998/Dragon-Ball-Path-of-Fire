using DragonBall.Bullet.ParentMVC;
using UnityEngine;

namespace DragonBall.Bullet.GuidedBulletMVC
{
    public class GuidedBulletView : BulletView
    {
        [SerializeField] private bool useRotation = true;

        private GuidedBulletController guidedController;

        public void SetGuidedController(GuidedBulletController controllerToSet) => guidedController = controllerToSet;

        protected override void Awake() => base.Awake();

        protected override void Update()
        {
            if (guidedController != null)
                guidedController.Update();
        }

        public Vector2 GetVelocity() => rb.linearVelocity;

        public void SetRotation(float angle)
        {
            if (useRotation)
                transform.rotation = Quaternion.Euler(0, 0, angle);

            UpdateSpriteFlip(rb.linearVelocity);
        }

        public override void SetVelocity(Vector2 velocity) => base.SetVelocity(velocity);
    }
}