using System.Collections.Generic;
using UnityEngine;

namespace DragonBall.Bullet
{
    public class BulletService
    {
        private Dictionary<BulletType, BulletPool> bulletPools = new Dictionary<BulletType, BulletPool>();
        private Dictionary<BulletType, GuidedBulletPool> guidedBulletPools = new Dictionary<BulletType, GuidedBulletPool>();

        public BulletService(
            Dictionary<BulletType, (BulletView, BulletScriptableObject)> bulletData,
            Dictionary<BulletType, (GuidedBulletView, GuidedBulletScriptableObject)> guidedBulletData = null)
        {
            InitializeRegularBulletPools(bulletData);
            InitializeGuidedBulletPools(guidedBulletData);
        }

        private void InitializeRegularBulletPools(Dictionary<BulletType, (BulletView, BulletScriptableObject)> bulletData)
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

        private void InitializeGuidedBulletPools(Dictionary<BulletType, (GuidedBulletView, GuidedBulletScriptableObject)> guidedBulletData)
        {
            if (guidedBulletData == null) return;

            foreach (var item in guidedBulletData)
            {
                BulletType type = item.Key;
                GuidedBulletView prefab = item.Value.Item1;
                GuidedBulletScriptableObject so = item.Value.Item2;

                GuidedBulletModel model = new GuidedBulletModel(
                    so.BulletSpeed,
                    so.BulletDamage,
                    so.BulletLifetime,
                    so.RotationSpeed,
                    so.GuidanceDelay,
                    so.MaxGuidanceTime);

                GuidedBulletPool pool = new GuidedBulletPool(prefab, model);
                guidedBulletPools[type] = pool;
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

        public void FireGuidedBullet(BulletType type, Vector2 position, Vector2 direction, Transform target, BulletTargetType targetType = BulletTargetType.Player)
        {
            if (guidedBulletPools.TryGetValue(type, out GuidedBulletPool pool))
            {
                GuidedBulletController bullet = pool.GetGuidedBullet();
                if (bullet != null)
                    bullet.Activate(position, direction, target, targetType);
            }
        }
    }
}