namespace DragonBall.Enemy
{
    public class EnemyModel
    {
        public EnemyType EnemyType { get; private set; }

        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public bool IsDead { get; set; }

        public float MovementSpeed { get; private set; }
        public float DetectionRange { get; private set; }

        public EnemyModel
        (
            EnemyType _enemyType,
            float _maxHealth,
            float _movementSpeed,
            float _detectionRange
        )
        {
            EnemyType = _enemyType;
            MaxHealth = _maxHealth;
            CurrentHealth = MaxHealth;
            IsDead = false;
            MovementSpeed = _movementSpeed;
            DetectionRange = _detectionRange;
        }

        public void TakeDamage(float damageAmount)
        {
            CurrentHealth -= damageAmount;
            if (CurrentHealth < 0)
                CurrentHealth = 0;
        }

        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
            IsDead = false;
        }
    }
}