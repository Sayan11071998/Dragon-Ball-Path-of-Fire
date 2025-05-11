using UnityEngine;
using DragonBall.Bullet.ParentMVC;
using DragonBall.Utilities;

namespace DragonBall.Bullet.BulletUtilities
{
    public class BulletPool : GenericObjectPool<BulletController>
    {
        protected BulletView bulletPrefab;
        protected BulletModel bulletModel;

        public BulletPool(BulletView bulletPrefabToSet, BulletModel bulletModelToSet)
        {
            bulletPrefab = bulletPrefabToSet;
            bulletModel = bulletModelToSet;
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