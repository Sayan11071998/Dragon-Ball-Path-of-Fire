using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyModel
    {
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        public float AttackDamage { get; private set; }
        public float AttackCooldown { get; private set; }

        public float lastAttackTime = -Mathf.Infinity;

        public EnemyModel(float _maxHealth, float _attackDamage, float _attackCooldown)
        {
            MaxHealth = _maxHealth;
            CurrentHealth = MaxHealth;
            AttackDamage = _attackDamage;
            AttackCooldown = _attackCooldown;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            Debug.Log($"Enemy Health: {CurrentHealth}");
            if (CurrentHealth < 0)
                CurrentHealth = 0;
        }

        public void Reset() => CurrentHealth = MaxHealth;
    }
}