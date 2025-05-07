using UnityEngine;

namespace DragonBall.Enemy
{
    public class FinalBossTypeEnemyModel : BaseEnemyModel
    {
        public bool IsEnraged { get; private set; } = false;
        public float EnragedDamageMultiplier { get; private set; } = 1.5f;
        public float EnragedAttackSpeedMultiplier { get; private set; } = 1.3f;

        public float LastSpecialAttackTime { get; set; } = -Mathf.Infinity;
        public float SpecialAttackCooldown { get; set; } = 15f;

        private float originalAttackDamage;
        private float originalAttackCooldown;

        public FinalBossTypeEnemyModel(float _maxHealth, float _attackDamage, float _attackCooldown)
            : base(_maxHealth, _attackDamage, _attackCooldown)
        {
            originalAttackDamage = _attackDamage;
            originalAttackCooldown = _attackCooldown;
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
        }
    }
}