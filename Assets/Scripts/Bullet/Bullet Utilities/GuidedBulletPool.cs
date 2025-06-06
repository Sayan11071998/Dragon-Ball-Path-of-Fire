using DragonBall.Bullet.GuidedBulletMVC;
using DragonBall.Bullet.ParentMVC;

namespace DragonBall.Bullet.BulletUtilities
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

        protected override BulletController CreateController(BulletView view) => new GuidedBulletController(guidedBulletModel, view as GuidedBulletView, this);
    }
}