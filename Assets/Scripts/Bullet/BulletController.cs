using DragonBall.Enemy;
using UnityEngine;

namespace DragonBall.Bullet
{
    public class BulletController
    {
        protected BulletModel bulletModel;
        protected BulletView bulletView;
        protected BulletPool bulletPool;

        protected float creationTime;

        public BulletController(BulletModel _bulletModel, BulletView _bulletView, BulletPool _bulletPool)
        {
            bulletModel = _bulletModel;
            bulletView = _bulletView;
            bulletPool = _bulletPool;

            bulletView.SetController(this);
        }

        public virtual void Activate(Vector2 position, Vector2 direction, BulletTargetType targetType = BulletTargetType.Enemy)
        {
            bulletView.transform.position = position;
            bulletView.gameObject.SetActive(true);
            bulletView.SetVelocity(direction * bulletModel.Speed);
            bulletView.SetTargetType(targetType);
            creationTime = Time.time;
        }

        public virtual void Update()
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

        public virtual void Deactivate()
        {
            bulletView.Deactivate();
            bulletPool.ReturnItem(this);
        }
    }
}