using UnityEngine;

namespace DragonBall.Enemy
{
    public class BaseEnemyModel
    {
        public float MaxHealth { get; protected set; }
        public float CurrentHealth { get; protected set; }

        public float AttackDamage { get; protected set; }
        public float AttackCooldown { get; protected set; }

        public float lastAttackTime = -Mathf.Infinity;

        public BaseEnemyModel(float _maxHealth, float _attackDamage, float _attackCooldown)
        {
            MaxHealth = _maxHealth;
            CurrentHealth = MaxHealth;
            AttackDamage = _attackDamage;
            AttackCooldown = _attackCooldown;
        }

        public virtual void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0)
                CurrentHealth = 0;
        }

        public virtual void Reset() => CurrentHealth = MaxHealth;
    }
}