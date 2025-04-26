using DragonBall.Core;
using DragonBall.Enemy;
using UnityEngine;

namespace DragonBall.Bullet
{
    public class BulletController
    {
        private BulletModel model;
        private BulletView view;

        private float creationTime;

        public BulletController(BulletModel _model, BulletView _view)
        {
            model = _model;
            view = _view;
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
            GameService.Instance.bulletService.ReturnBulletToPool(this);
        }
    }
}