using UnityEngine;
using DragonBall.Player.PlayerData;

namespace DragonBall.Player.PlayerMVC
{
    public class PlayerModel
    {
        private PlayerScriptableObject config;
        private bool isSuperSaiyanMode = false;

        public float CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }

        public float CurrentStamina { get; private set; }
        public bool HasEnoughStaminaForKamehameha => CurrentStamina >= config.KamehamehaStaminaCost;

        public float MaxHealth { get; private set; }
        public float MaxStamina { get; private set; }
        public float MoveSpeed { get; private set; }
        public float FlySpeed { get; private set; }
        public int KickAttackPower { get; private set; }

        public bool IsFlying { get; set; }
        public float LastDodgeTime { get; set; } = -10f;
        public bool IsDodging { get; set; }
        public float DodgeEndTime { get; set; }
        public float LastKickTime { get; set; } = -10f;
        public bool IsKickOnCooldown => Time.time < LastKickTime + config.KickAttackCooldown;
        public float LastFireTime { get; set; } = -10f;
        public bool IsFireOnCooldown => Time.time < LastFireTime + config.FireCooldown;
        public bool IsGrounded { get; set; }
        public bool IsFacingRight { get; set; } = true;
        public int JumpCount { get; set; } = 0;
        public int DragonBallCount { get; private set; } = 0;

        public float JumpSpeed => config.JumpSpeed;
        public float JumpHorizontalDampening => config.JumpHorizontalDampening;
        public float VanishRange => config.VanishRange;
        public float DodgeSpeed => config.DodgeSpeed;
        public float DodgeDuration => config.DodgeDuration;
        public float DodgeCooldown => config.DodgeCooldown;
        public float KickAttackRange => config.KickAttackRange;
        public int DragonBallsRequiredForSuperSaiyan => config.DragonBallsRequiredForSuperSaiyan;

        public float SuperSaiyanHealthMultiplier => config.SuperSaiyanHealthMultiplier;
        public float SuperSaiyanStaminaMultiplier => config.SuperSaiyanStaminaMultiplier;
        public float SuperSaiyanSpeedMultiplier => config.SuperSaiyanSpeedMultiplier;
        public float SuperSaiyanPowerMultiplier => config.SuperSaiyanPowerMultiplier;

        public PlayerModel(PlayerScriptableObject playerConfig)
        {
            config = playerConfig;

            MaxHealth = config.PlayerHealth;
            CurrentHealth = MaxHealth;
            IsDead = false;

            MaxStamina = config.PlayerStamina;
            CurrentStamina = MaxStamina;

            MoveSpeed = config.MoveSpeed;
            FlySpeed = config.FlySpeed;
            IsFlying = false;

            IsDodging = false;
            KickAttackPower = config.KickAttackPower;

            IsGrounded = true;
            IsFacingRight = true;
            JumpCount = 0;
            DragonBallCount = 0;
        }

        public bool IsSuperSaiyan() => isSuperSaiyanMode;

        public void IncrementDragonBallCount() => DragonBallCount++;

        public void ApplySuperSaiyanBuffs()
        {
            MaxHealth = config.PlayerHealth * SuperSaiyanHealthMultiplier;
            CurrentHealth = MaxHealth;

            MaxStamina = config.PlayerStamina * SuperSaiyanStaminaMultiplier;
            CurrentStamina = MaxStamina;

            MoveSpeed = config.MoveSpeed * SuperSaiyanSpeedMultiplier;
            FlySpeed = config.FlySpeed * SuperSaiyanSpeedMultiplier;

            KickAttackPower = (int)(config.KickAttackPower * SuperSaiyanPowerMultiplier);

            if (IsDead) IsDead = false;

            isSuperSaiyanMode = true;
        }

        public void RemoveSuperSaiyanBuffs()
        {
            float healthPercentage = CurrentHealth / MaxHealth;
            float staminaPercentage = CurrentStamina / MaxStamina;

            MoveSpeed = config.MoveSpeed;
            FlySpeed = config.FlySpeed;
            KickAttackPower = config.KickAttackPower;
            IsFlying = false;

            MaxHealth = config.PlayerHealth;
            CurrentHealth = MaxHealth * healthPercentage;
            MaxStamina = config.PlayerStamina;
            CurrentStamina = MaxStamina * staminaPercentage;

            if (CurrentHealth <= 0) CurrentHealth = 1;

            isSuperSaiyanMode = false;
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
            if (CurrentStamina < config.KamehamehaStaminaCost)
                return false;

            CurrentStamina -= config.KamehamehaStaminaCost;
            return true;
        }

        public void RegenerateStamina(float deltaTime)
        {
            if (CurrentStamina < MaxStamina)
                CurrentStamina = Mathf.Min(MaxStamina, CurrentStamina + (config.StaminaRegenRate * deltaTime));
        }

        public void Reset()
        {
            CurrentHealth = MaxHealth;
            CurrentStamina = MaxStamina;
            IsDead = false;
        }
    }
}