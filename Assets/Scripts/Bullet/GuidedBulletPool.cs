using UnityEngine;

namespace DragonBall.Bullet
{
    public class GuidedBulletPool : BulletPool
    {
        private GuidedBulletModel guidedBulletModel;

        public GuidedBulletPool(GuidedBulletView bulletPrefab, GuidedBulletModel bulletModel)
            : base(bulletPrefab, bulletModel)
        {
            guidedBulletModel = bulletModel;
        }

        public GuidedBulletController GetGuidedBullet() => GetItem<GuidedBulletController>() as GuidedBulletController;

        protected override BulletController CreateController(BulletView view)
        {
            return new GuidedBulletController(guidedBulletModel, view as GuidedBulletView, this);
        }
    }
}