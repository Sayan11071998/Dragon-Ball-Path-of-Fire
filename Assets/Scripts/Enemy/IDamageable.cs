namespace DragonBall.Enemy
{
    public interface IDamageable
    {
        void Damage(float damageAmount);
        bool IsDead { get; }
        void ReturnToPool();
    }
}