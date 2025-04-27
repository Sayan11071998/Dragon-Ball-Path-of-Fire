namespace DragonBall.Enemy
{
    public class EnemyModel
    {
        public EnemyType Type => Data.enemyType;
        public EnemyScriptableObject Data { get; }
        public float CurrentHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0;

        public EnemyModel(EnemyScriptableObject data)
        {
            Data = data;
            CurrentHealth = data.maxHealth;
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
        }
    }
}