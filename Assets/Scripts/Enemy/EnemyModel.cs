namespace DragonBall.Enemy
{
    public class EnemyModel
    {
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        public EnemyModel(float _maxHealth)
        {
            MaxHealth = _maxHealth;
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0)
                CurrentHealth = 0;
        }

        public void Reset() => CurrentHealth = MaxHealth;
    }
}