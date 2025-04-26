using DragonBall.Core;
using DragonBall.Enemy;
using UnityEngine;

namespace DragonBall.Bullet
{
    public class BulletController
    {
        private BulletModel model;
        private BulletView view;
        private BulletPool pool;
        private float creationTime;

        public BulletController(BulletModel _model, BulletView _view, BulletPool _pool)
        {
            model = _model;
            view = _view;
            pool = _pool;
            view.SetController(this);
        }

        public void Activate(Vector2 position, Vector2 direction)
        {
            view.transform.position = position;
            view.gameObject.SetActive(true);
            view.SetVelocity(direction * model.Speed);
            creationTime = Time.time;
        }

        public void Update()
        {
            if (Time.time > creationTime + model.Lifetime)
                Deactivate();
        }

        public void OnCollision(IDamageable target)
        {
            target.Damage(model.Damage);
            Deactivate();
        }

        public void Deactivate()
        {
            view.Deactivate();
            pool.ReturnItem(this);
            // GameService.Instance.bulletService.ReturnBulletToPool(this);
        }
    }
}