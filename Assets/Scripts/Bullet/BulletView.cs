using DragonBall.Enemy;
using DragonBall.Core;
using UnityEngine;

namespace DragonBall.Bullet
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private BulletTargetType targetType = BulletTargetType.Enemy;

        private BulletController bulletController;

        protected Rigidbody2D rb;

        public void SetController(BulletController controllerToSet) => bulletController = controllerToSet;

        protected virtual void Awake() => rb = GetComponent<Rigidbody2D>();

        public void SetVelocity(Vector2 velocity) => rb.linearVelocity = velocity;

        public void Deactivate() => gameObject.SetActive(false);

        public void SetTargetType(BulletTargetType type) => targetType = type;

        protected virtual void Update() => bulletController.Update();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (targetType == BulletTargetType.Enemy && collision.gameObject.TryGetComponent<IDamageable>(out var target))
            {
                bulletController.OnCollision(target);
                return;
            }

            if (targetType == BulletTargetType.Player && collision.CompareTag("Player"))
            {
                var playerController = GameService.Instance.playerService.PlayerController;
                if (!playerController.PlayerModel.IsDead)
                {
                    playerController.TakeDamage(bulletController.GetDamage());
                    bulletController.Deactivate();
                }
                return;
            }
        }
    }
}