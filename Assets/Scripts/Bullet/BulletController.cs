using DragonBall.Enemy;
using UnityEngine;

namespace DragonBall.Bullet
{
    public class BulletController
    {
        private BulletModel bulletModel;
        private BulletView bulletView;
        private BulletPool bulletPool;

        private float creationTime;

        public BulletController(BulletModel _bulletModel, BulletView _bulletView, BulletPool _bulletPool)
        {
            bulletModel = _bulletModel;
            bulletView = _bulletView;
            bulletPool = _bulletPool;

            bulletView.SetController(this);
        }

        public void Activate(Vector2 position, Vector2 direction, BulletTargetType targetType = BulletTargetType.Enemy)
        {
            bulletView.transform.position = position;
            bulletView.gameObject.SetActive(true);
            bulletView.SetVelocity(direction * bulletModel.Speed);
            bulletView.SetTargetType(targetType);
            creationTime = Time.time;
        }

        public void Update()
        {
            if (Time.time > creationTime + bulletModel.Lifetime)
                Deactivate();
        }

        public void OnCollision(IDamageable target)
        {
            target.Damage(bulletModel.Damage);
            Deactivate();
        }

        public float GetDamage() => bulletModel.Damage;

        public void Deactivate()
        {
            bulletView.Deactivate();
            bulletPool.ReturnItem(this);
        }
    }
}