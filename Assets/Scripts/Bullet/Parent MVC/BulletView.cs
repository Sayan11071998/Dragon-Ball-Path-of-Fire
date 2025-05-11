using UnityEngine;
using DragonBall.Bullet.BulletData;
using DragonBall.Enemy.EnemyUtilities;
using DragonBall.Core;
using DragonBall.GameStrings;

namespace DragonBall.Bullet.ParentMVC
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private BulletTargetType targetType = BulletTargetType.Enemy;
        [SerializeField] protected bool flipSpriteWithDirection = true;

        private BulletController bulletController;

        protected Rigidbody2D rb;
        protected SpriteRenderer spriteRenderer;

        public void SetController(BulletController controllerToSet) => bulletController = controllerToSet;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void SetVelocity(Vector2 velocity)
        {
            rb.linearVelocity = velocity;
            UpdateSpriteFlip(velocity);
        }

        protected void UpdateSpriteFlip(Vector2 velocity)
        {
            if (flipSpriteWithDirection && spriteRenderer != null && velocity.magnitude > 0.1f)
                spriteRenderer.flipX = velocity.x < 0;
        }

        public void Deactivate() => gameObject.SetActive(false);

        public void SetTargetType(BulletTargetType type) => targetType = type;

        protected virtual void Update() => bulletController.Update();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(GameString.PlatformTag))
            {
                bulletController.Deactivate();
                return;
            }

            if (targetType == BulletTargetType.Enemy && collision.gameObject.TryGetComponent<IDamageable>(out var target))
            {
                bulletController.OnCollision(target);
                return;
            }

            if (targetType == BulletTargetType.Player && collision.CompareTag(GameString.PlayerTag))
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