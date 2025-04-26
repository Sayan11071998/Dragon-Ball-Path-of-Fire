using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyScript : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100;

        private float currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void Damage(float DamageAmount)
        {
            currentHealth -= DamageAmount;
            Debug.Log($"Enemy took {DamageAmount} damage! Remaining health: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Enemy defeated!");
            Destroy(gameObject);
        }
    }
}