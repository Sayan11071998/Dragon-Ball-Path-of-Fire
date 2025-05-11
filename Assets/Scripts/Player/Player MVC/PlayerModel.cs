using UnityEngine;

namespace DragonBall.Player.PlayerMVC
{
    public class PlayerModel
    {
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public float OriginalMaxHealth { get; private set; }
        public bool IsDead { get; private set; }

        public float MaxStamina { get; private set; }
        public float CurrentStamina { get; private set; }
        public float OriginalMaxStamina { get; private set; }
        public float StaminaRegenRate { get; private set; }
        public float KamehamehaStaminaCost { get; private set; }
        public bool HasEnoughStaminaForKamehameha => CurrentStamina >= KamehamehaStaminaCost;

        public float MoveSpeed { get; private set; }
        public float OriginalMoveSpeed { get; private set; }

        public float JumpSpeed { get; private set; }
        public float JumpHorizontalDampening { get; private set; }

        public float FlySpeed { get; private set; }
        public float OriginalFlySpeed { get; private set; }
        public bool IsFlying { get; set; }

        public float VanishRange { get; private set; }

        public float DodgeSpeed { get; private set; }
        public float DodgeDuration { get; private set; }
        public float DodgeCooldown { get; private set; }
        public float LastDodgeTime { get; set; } = -10f;
        public bool IsDodging { get; set; }
        public float DodgeEndTime { get; set; }

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

        public int DragonBallCount { get; private set; } = 0;
        public int DragonBallsRequiredForSuperSaiyan { get; private set; }

        public float SuperSaiyanHealthMultiplier { get; private set; }
        public float SuperSaiyanStaminaMultiplier { get; private set; }
        public float SuperSaiyanSpeedMultiplier { get; private set; }
        public float SuperSaiyanPowerMultiplier { get; private set; }

        public PlayerModel(
            float _maxHealth,
            float _maxStamina,
            float _staminaRegenRate,
            float _moveSpeed,
            float _jumpSpeed,
            float _jumpHorizontalDampening,
            float _flySpeed,
            float _vanishRange,
            float _dodgeSpeed,
            float _dodgeDuration,
            float _dodgeCooldown,
            int _kickAttackPower,
            float _kickAttackRange,
            float _kickAttackCooldown,
            float _fireCooldown,
            int _dragonBallsRequiredForSuperSaiyan,
            float _superSaiyanHealthMultiplier,
            float _superSaiyanStaminaMultiplier,
            float _superSaiyanSpeedMultiplier,
            float _superSaiyanPowerMultiplier,
            float _kamehamehaStaminaCost
        )
        {
            OriginalMaxHealth = _maxHealth;
            MaxHealth = _maxHealth;
            CurrentHealth = MaxHealth;
            IsDead = false;

            OriginalMaxStamina = _maxStamina;
            MaxStamina = _maxStamina;
            CurrentStamina = MaxStamina;
            StaminaRegenRate = _staminaRegenRate;
            KamehamehaStaminaCost = _kamehamehaStaminaCost;

            OriginalMoveSpeed = _moveSpeed;
            MoveSpeed = _moveSpeed;

            JumpSpeed = _jumpSpeed;
            JumpHorizontalDampening = _jumpHorizontalDampening;

            OriginalFlySpeed = _flySpeed;
            FlySpeed = _flySpeed;
            IsFlying = false;

            VanishRange = _vanishRange;

            DodgeSpeed = _dodgeSpeed;
            DodgeDuration = _dodgeDuration;
            DodgeCooldown = _dodgeCooldown;
            IsDodging = false;

            KickAttackPower = _kickAttackPower;
            OriginalKickAttackPower = _kickAttackPower;
            KickAttackRange = _kickAttackRange;
            KickAttackCooldown = _kickAttackCooldown;

            FireCooldown = _fireCooldown;

            DragonBallsRequiredForSuperSaiyan = _dragonBallsRequiredForSuperSaiyan;

            SuperSaiyanHealthMultiplier = _superSaiyanHealthMultiplier;
            SuperSaiyanStaminaMultiplier = _superSaiyanStaminaMultiplier;
            SuperSaiyanSpeedMultiplier = _superSaiyanSpeedMultiplier;
            SuperSaiyanPowerMultiplier = _superSaiyanPowerMultiplier;

            IsGrounded = true;
            IsFacingRight = true;
            JumpCount = 0;

            DragonBallCount = 0;
        }

        public void IncrementDragonBallCount() => DragonBallCount++;

        public void ApplySuperSaiyanBuffs()
        {
            MaxHealth = OriginalMaxHealth * SuperSaiyanHealthMultiplier;
            CurrentHealth = MaxHealth;
            MaxStamina = OriginalMaxStamina * SuperSaiyanStaminaMultiplier;
            CurrentStamina = MaxStamina;
            MoveSpeed = OriginalMoveSpeed * SuperSaiyanSpeedMultiplier;
            FlySpeed = OriginalFlySpeed * SuperSaiyanSpeedMultiplier;
            KickAttackPower = (int)(OriginalKickAttackPower * SuperSaiyanPowerMultiplier);

            if (IsDead)
                IsDead = false;
        }

        public void RemoveSuperSaiyanBuffs()
        {
            MoveSpeed = OriginalMoveSpeed;
            KickAttackPower = OriginalKickAttackPower;
            IsFlying = false;

            float healthPercentage = CurrentHealth / MaxHealth;
            MaxHealth = OriginalMaxHealth;
            CurrentHealth = MaxHealth * healthPercentage;

            if (CurrentHealth <= 0)
                CurrentHealth = 1;

            float staminaPercentage = CurrentStamina / MaxStamina;
            MaxStamina = OriginalMaxStamina;
            CurrentStamina = MaxStamina * staminaPercentage;

            FlySpeed = OriginalFlySpeed;
        }

        public void TakeDamage(float damage)
        {
            if (damage <= 0 || IsDead) return;

            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                IsDead = true;
            }
        }

        public bool UseStaminaForKamehameha()
        {
            if (CurrentStamina < KamehamehaStaminaCost)
                return false;

            CurrentStamina -= KamehamehaStaminaCost;
            return true;
        }

        public void RegenerateStamina(float deltaTime)
        {
            if (CurrentStamina < MaxStamina)
                CurrentStamina = Mathf.Min(MaxStamina, CurrentStamina + (StaminaRegenRate * deltaTime));
        }

        public void Reset()
        {
            CurrentHealth = MaxHealth;
            CurrentStamina = MaxStamina;
            IsDead = false;
        }
    }
}