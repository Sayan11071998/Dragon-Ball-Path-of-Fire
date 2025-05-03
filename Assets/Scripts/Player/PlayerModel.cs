using UnityEngine;

namespace DragonBall.Player
{
    public class PlayerModel
    {
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public float OriginalMaxHealth { get; private set; }
        public bool IsDead { get; private set; }

        public float MoveSpeed { get; private set; }
        public float OriginalMoveSpeed { get; private set; }

        public float JumpSpeed { get; private set; }
        public float JumpHorizontalDampening { get; private set; }

        public float VanishRange { get; private set; }

        public float DodgeSpeed { get; private set; }
        public float DodgeDuration { get; private set; }
        public float DodgeCooldown { get; private set; }

        public int KickAttackPower { get; private set; }
        public int OriginalKickAttackPower { get; private set; }
        public float KickAttackRange { get; private set; }
        public float KickAttackCooldown { get; private set; }
        public float LastKickTime { get; set; } = -10f;
        public bool IsKickOnCooldown => Time.time < LastKickTime + KickAttackCooldown;

        public float FireCooldown { get; private set; }
        public float LastFireTime { get; set; } = -10f;
        public bool IsFireOnCooldown => Time.time < LastFireTime + FireCooldown;

        public bool IsGrounded { get; set; }
        public bool IsFacingRight { get; set; } = true;
        public int JumpCount { get; set; } = 0;

        public float LastDodgeTime { get; set; } = -10f;
        public bool IsDodging { get; set; }
        public float DodgeEndTime { get; set; }

        public int DragonBallCount { get; private set; } = 0;
        public int DragonBallsRequiredForSuperSaiyan { get; private set; }

        public float SuperSaiyanSpeedMultiplier { get; private set; }
        public float SuperSaiyanPowerMultiplier { get; private set; }
        public float SuperSaiyanHealthMultiplier { get; private set; }

        public PlayerModel
        (
            float _maxHealth,
            float _moveSpeed,
            float _jumpSpeed,
            float _jumpHorizontalDampening,
            float _vanishRange,
            float _dodgeSpeed,
            float _dodgeDuration,
            float _dodgeCooldown,
            int _kickAttackPower,
            float _kickAttackRange,
            float _kickAttackCooldown,
            float _fireCooldown,
            int _dragonBallsRequiredForSuperSaiyan,
            float _superSaiyanSpeedMultiplier,
            float _superSaiyanPowerMultiplier,
            float _superSaiyanHealthMultiplier
        )
        {
            MaxHealth = _maxHealth;
            OriginalMaxHealth = _maxHealth;
            CurrentHealth = MaxHealth;
            MoveSpeed = _moveSpeed;
            OriginalMoveSpeed = _moveSpeed;
            JumpSpeed = _jumpSpeed;
            JumpHorizontalDampening = _jumpHorizontalDampening;
            VanishRange = _vanishRange;
            DodgeSpeed = _dodgeSpeed;
            DodgeDuration = _dodgeDuration;
            DodgeCooldown = _dodgeCooldown;
            KickAttackPower = _kickAttackPower;
            OriginalKickAttackPower = _kickAttackPower;
            KickAttackRange = _kickAttackRange;
            KickAttackCooldown = _kickAttackCooldown;
            FireCooldown = _fireCooldown;
            DragonBallsRequiredForSuperSaiyan = _dragonBallsRequiredForSuperSaiyan;
            SuperSaiyanSpeedMultiplier = _superSaiyanSpeedMultiplier;
            SuperSaiyanPowerMultiplier = _superSaiyanPowerMultiplier;
            SuperSaiyanHealthMultiplier = _superSaiyanHealthMultiplier;
            IsDead = false;
            IsGrounded = true;
            IsDodging = false;
        }

        public void IncrementDragonBallCount() => DragonBallCount++;

        public void ApplySuperSaiyanBuffs()
        {
            MoveSpeed = OriginalMoveSpeed * SuperSaiyanSpeedMultiplier;
            KickAttackPower = (int)(OriginalKickAttackPower * SuperSaiyanPowerMultiplier);
            MaxHealth = OriginalMaxHealth * SuperSaiyanHealthMultiplier;
            CurrentHealth = MaxHealth;

            if (IsDead)
                IsDead = false;
        }

        public void RemoveSuperSaiyanBuffs()
        {
            MoveSpeed = OriginalMoveSpeed;
            KickAttackPower = OriginalKickAttackPower;

            float healthPercentage = CurrentHealth / MaxHealth;
            MaxHealth = OriginalMaxHealth;
            CurrentHealth = MaxHealth * healthPercentage;

            if (CurrentHealth <= 0)
                CurrentHealth = 1;
        }

        public void TakeDamage(float damage)
        {
            if (damage <= 0 || IsDead) return;

            CurrentHealth -= damage;
            Debug.Log($"Player Health: {CurrentHealth}");
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                IsDead = true;
            }
        }

        public void Reset()
        {
            CurrentHealth = MaxHealth;
            IsDead = false;
        }
    }
}