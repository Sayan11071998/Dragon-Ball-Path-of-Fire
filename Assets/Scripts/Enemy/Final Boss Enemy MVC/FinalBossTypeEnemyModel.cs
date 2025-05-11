using DragonBall.Enemy.ParentMVC;
using UnityEngine;

namespace DragonBall.Enemy.FinalBossEnemyMVC
{
    public class FinalBossTypeEnemyModel : BaseEnemyModel
    {
        public bool IsEnraged { get; private set; } = false;
        public float EnragedDamageMultiplier { get; private set; } = 1.5f;
        public float EnragedAttackSpeedMultiplier { get; private set; } = 1.3f;

        public float LastSpecialAttackTime { get; set; } = -Mathf.Infinity;
        public float SpecialAttackCooldown { get; set; } = 15f;

        public bool HasRegeneratedHealth { get; private set; } = false;
        public bool IsRegenerating { get; private set; } = false;

        private float originalAttackDamage;
        private float originalAttackCooldown;

        public FinalBossTypeEnemyModel(float _maxHealth, float _attackDamage, float _attackCooldown)
            : base(_maxHealth, _attackDamage, _attackCooldown)
        {
            originalAttackDamage = _attackDamage;
            originalAttackCooldown = _attackCooldown;
        }

        public override void TakeDamage(float damage)
        {
            if (IsRegenerating) return;
            
            CurrentHealth -= damage;
            if (CurrentHealth < 0)
                CurrentHealth = 0;
        }

        public void EnterEnragedState()
        {
            if (!IsEnraged)
            {
                IsEnraged = true;

                AttackDamage = originalAttackDamage * EnragedDamageMultiplier;
                AttackCooldown = originalAttackCooldown / EnragedAttackSpeedMultiplier;
            }
        }

        public void RegenerateHealth()
        {
            if (!HasRegeneratedHealth)
            {
                IsRegenerating = true;
                HasRegeneratedHealth = true;
            }
        }

        public void CompleteRegeneration()
        {
            IsRegenerating = false;
            CurrentHealth = MaxHealth;
        }

        public void ResetEnragedState()
        {
            IsEnraged = false;
            AttackDamage = originalAttackDamage;
            AttackCooldown = originalAttackCooldown;
        }

        public override void Reset()
        {
            base.Reset();
            ResetEnragedState();
            LastSpecialAttackTime = -Mathf.Infinity;
            HasRegeneratedHealth = false;
            IsRegenerating = false;
        }
    }
}