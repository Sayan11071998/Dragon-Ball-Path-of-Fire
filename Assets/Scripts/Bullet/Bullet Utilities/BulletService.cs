using System.Collections.Generic;
using DragonBall.Bullet.BulletData;
using DragonBall.Bullet.GuidedBulletMVC;
using DragonBall.Bullet.ParentMVC;
using UnityEngine;

namespace DragonBall.Bullet.BulletUtilities.BulletUtilities
{
    public class BulletService
    {
        private Dictionary<BulletType, BulletPool> bulletPool = new Dictionary<BulletType, BulletPool>();
        private Dictionary<BulletType, GuidedBulletPool> guidedBulletPool = new Dictionary<BulletType, GuidedBulletPool>();

        public BulletService(
            Dictionary<BulletType, (BulletView, BulletScriptableObject)> _bulletData,
            Dictionary<BulletType, (GuidedBulletView, GuidedBulletScriptableObject)> _guidedBulletData = null)
        {
            InitializeRegularBulletPools(_bulletData);
            InitializeGuidedBulletPools(_guidedBulletData);
        }

        private void InitializeRegularBulletPools(Dictionary<BulletType, (BulletView, BulletScriptableObject)> _regularBulletData)
        {
            foreach (var item in _regularBulletData)
            {
                BulletType bulletType = item.Key;
                BulletView bulletPrefab = item.Value.Item1;
                BulletScriptableObject bulletScriptableObject = item.Value.Item2;

                BulletModel model = new BulletModel(bulletScriptableObject.BulletSpeed, bulletScriptableObject.BulletDamage, bulletScriptableObject.BulletLifetime);
                BulletPool pool = new BulletPool(bulletPrefab, model);
                bulletPool[bulletType] = pool;
            }
        }

        private void InitializeGuidedBulletPools(Dictionary<BulletType, (GuidedBulletView, GuidedBulletScriptableObject)> _guidedBulletData)
        {
            if (_guidedBulletData == null) return;

            foreach (var item in _guidedBulletData)
            {
                BulletType bulletType = item.Key;
                GuidedBulletView bulletPrefab = item.Value.Item1;
                GuidedBulletScriptableObject guidedBulletScriptableObject = item.Value.Item2;

                GuidedBulletModel model = new GuidedBulletModel(
                    guidedBulletScriptableObject.BulletSpeed,
                    guidedBulletScriptableObject.BulletDamage,
                    guidedBulletScriptableObject.BulletLifetime,
                    guidedBulletScriptableObject.RotationSpeed,
                    guidedBulletScriptableObject.GuidanceDelay,
                    guidedBulletScriptableObject.MaxGuidanceTime);

                GuidedBulletPool pool = new GuidedBulletPool(bulletPrefab, model);
                guidedBulletPool[bulletType] = pool;
            }
        }

        public void FireBullet(BulletType type, Vector2 position, Vector2 direction, BulletTargetType targetType = BulletTargetType.Enemy)
        {
            if (bulletPool.TryGetValue(type, out BulletPool pool))
            {
                BulletController bullet = pool.GetBullet();

                if (bullet != null)
                    bullet.Activate(position, direction, targetType);
            }
        }

        public void FireGuidedBullet(BulletType type, Vector2 position, Vector2 direction, Transform target,
            BulletTargetType targetType = BulletTargetType.Player, Vector3? targetOffset = null)
        {
            if (guidedBulletPool.TryGetValue(type, out GuidedBulletPool pool))
            {
                GuidedBulletController bullet = pool.GetGuidedBullet();

                if (bullet != null)
                {
                    bullet.Activate(position, direction, target, targetType);

                    if (targetOffset.HasValue)
                        bullet.SetTargetOffset(targetOffset.Value);
                }
            }
        }
    }
}