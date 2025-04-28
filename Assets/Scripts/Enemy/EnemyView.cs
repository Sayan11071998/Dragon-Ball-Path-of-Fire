using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyView : MonoBehaviour, IDamageable
    {
        private EnemyController controller;
        private Rigidbody2D rb;

        public void SetController(EnemyController controllerToSet) => controller = controllerToSet;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Damage(float damageAmount)
        {
            controller?.TakeDamage(damageAmount);
        }
    }
}