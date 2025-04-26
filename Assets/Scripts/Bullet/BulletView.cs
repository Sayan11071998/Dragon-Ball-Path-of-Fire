using DragonBall.Enemy;
using UnityEngine;

namespace DragonBall.Bullet
{
    public class BulletView : MonoBehaviour
    {
        private BulletController controller;
        private Rigidbody2D rb;

        public void SetController(BulletController controllerToSet) => controller = controllerToSet;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void SetVelocity(Vector2 velocity)
        {
            rb.linearVelocity = velocity;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            controller.Update();
        }

        private void OnTriggerEnter2D(Collider2D  collision)
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var target))
            {
                controller.OnCollision(target);
            }
        }
    }
}