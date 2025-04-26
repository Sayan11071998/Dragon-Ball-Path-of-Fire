using UnityEngine;

namespace DragonBall.Bullet
{
    public class BulletService
    {
        private BulletPool bulletPool;
        private BulletModel bulletModel;

        public BulletService(BulletView bulletPrefab, BulletScriptableObject _config)
        {
            bulletModel = new BulletModel(_config.BulletSpeed, _config.BulletDamage, _config.BulletLifetime);
            bulletPool = new BulletPool(bulletPrefab, bulletModel);
        }

        public void FireBullet(Vector2 position, Vector2 direction)
        {
            BulletController bullet = bulletPool.GetBullet();

            if (bullet != null)
                bullet.Activate(position, direction);
        }

        public void ReturnBulletToPool(BulletController bullet) => bulletPool.ReturnItem(bullet);
    }
}