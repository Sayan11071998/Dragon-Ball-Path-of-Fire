using UnityEngine;

namespace DragonBall.Bullet
{
    public class BulletService
    {
        private BulletPool bulletPool;
        private BulletModel bulletModel;

        public BulletService(BulletView bulletPrefab, BulletScriptableObject bulletConfig)
        {
            bulletModel = new BulletModel(bulletConfig);
            bulletPool = new BulletPool(bulletPrefab, bulletModel);
        }

        public void FireBullet(Vector2 position, Vector2 direction)
        {
            BulletController bullet = bulletPool.GetBullet();
            if (bullet != null)
            {
                bullet.Activate(position, direction);
            }
            else
            {
                Debug.LogError("Failed to get bullet from pool");
            }
        }

        public void ReturnBulletToPool(BulletController bullet)
        {
            bulletPool.ReturnItem(bullet);
        }
    }
}