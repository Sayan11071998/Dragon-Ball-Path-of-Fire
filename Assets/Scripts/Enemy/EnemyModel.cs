using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyModel
    {
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }

        public float MoveSpeed { get; private set; }

        public float AttackPower { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackCooldown { get; private set; }
        public float LastAttackTime { get; set; } = -10f;

        public float SensingRange { get; private set; }
        public float StoppingDistance { get; private set; }

        public bool IsAttackOnCooldown => Time.time < LastAttackTime + AttackCooldown;

        public EnemyModel(
            int health,
            float moveSpeed,
            float attackPower,
            float attackRange,
            float attackCooldown,
            float sensingRange,
            float stoppingDistance)
        {
            MaxHealth = health;
            Health = health;
            MoveSpeed = moveSpeed;
            AttackPower = attackPower;
            AttackRange = attackRange;
            AttackCooldown = attackCooldown;
            SensingRange = sensingRange;
            StoppingDistance = stoppingDistance;
        }

        public void TakeDamage(int damage) => Health = Mathf.Max(0, Health - damage);

        public bool IsDead => Health <= 0;
    }
}