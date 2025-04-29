using System.Collections.Generic;
using UnityEngine;

namespace DragonBall.Bullet
{
    public class BulletService
    {
        private Dictionary<BulletType, BulletPool> bulletPools = new Dictionary<BulletType, BulletPool>();

        public BulletService(Dictionary<BulletType, (BulletView, BulletScriptableObject)> bulletData)
        {
            foreach (var item in bulletData)
            {
                BulletType type = item.Key;
                BulletView prefab = item.Value.Item1;
                BulletScriptableObject so = item.Value.Item2;
                BulletModel model = new BulletModel(so.BulletSpeed, so.BulletDamage, so.BulletLifetime);
                BulletPool pool = new BulletPool(prefab, model);
                bulletPools[type] = pool;
            }
        }

        public void FireBullet(BulletType type, Vector2 position, Vector2 direction, BulletTargetType targetType = BulletTargetType.Enemy)
        {
            if (bulletPools.TryGetValue(type, out BulletPool pool))
            {
                BulletController bullet = pool.GetBullet();

                if (bullet != null)
                    bullet.Activate(position, direction, targetType);
            }
        }
    }
}