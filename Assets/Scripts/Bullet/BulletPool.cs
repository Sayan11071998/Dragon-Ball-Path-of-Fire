using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Bullet
{
    public class BulletPool : GenericObjectPool<BulletController>
    {
        private BulletView bulletPrefab;
        private BulletModel bulletModel;

        public BulletPool(BulletView bulletPrefab, BulletModel bulletModel)
        {
            this.bulletPrefab = bulletPrefab;
            this.bulletModel = bulletModel;
        }

        public BulletController GetBullet() => GetItem<BulletController>();

        protected override BulletController CreateItem<T>()
        {
            BulletView view = Object.Instantiate(bulletPrefab);
            return new BulletController(bulletModel, view, this);
        }
    }
}