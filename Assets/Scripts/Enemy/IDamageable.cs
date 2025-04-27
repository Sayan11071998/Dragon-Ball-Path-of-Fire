using UnityEngine;

namespace DragonBall.Core
{
    public interface IDamageable
    {
        void Damage(float damageAmount);
        bool IsDead { get; }
        void ReturnToPool();
    }
}