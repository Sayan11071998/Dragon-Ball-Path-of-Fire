using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Bullet
{
    public class BulletPool : GenericObjectPool<BulletController>
    {
        protected BulletView bulletPrefab;
        protected BulletModel bulletModel;

        public BulletPool(BulletView bulletPrefab, BulletModel bulletModel)
        {
            this.bulletPrefab = bulletPrefab;
            this.bulletModel = bulletModel;
        }

        public BulletController GetBullet() => GetItem<BulletController>();

        protected override BulletController CreateItem<T>()
        {
            BulletView view = Object.Instantiate(bulletPrefab);
            return CreateController(view);
        }

        protected virtual BulletController CreateController(BulletView view) => new BulletController(bulletModel, view, this);
    }
}