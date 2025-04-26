namespace DragonBall.Player
{
    public class PlayerModel
    {
        public int Health { get; private set; }

        public float MoveSpeed { get; private set; }

        public float JumpSpeed { get; private set; }
        public float JumpHorizontalDampening { get; private set; }

        public float VanishRange { get; private set; }

        public float DodgeSpeed { get; private set; }
        public float DodgeDuration { get; private set; }
        public float DodgeCooldown { get; private set; }

        public int KickAttackPower { get; private set; }
        public float KickAttackRange { get; private set; }
        public float KickAttackCooldown { get; private set; }
        public float LastKickTime { get; set; } = -10f;
        public bool IsKickOnCooldown => UnityEngine.Time.time < LastKickTime + KickAttackCooldown;

        public float FireCooldown { get; private set; }
        public float LastFireTime { get; set; } = -10f;
        public bool IsFireOnCooldown => UnityEngine.Time.time < LastFireTime + FireCooldown;

        public bool IsGrounded { get; set; }
        public bool IsFacingRight { get; set; } = true;
        public int JumpCount { get; set; } = 0;

        public float LastDodgeTime { get; set; } = -10f;
        public bool IsDodging { get; set; }
        public float DodgeEndTime { get; set; }

        public int DragonBallCount { get; private set; } = 0;

        public PlayerModel
        (
            int _Health,
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
            float _fireCooldown
        )
        {
            Health = _Health;
            MoveSpeed = _moveSpeed;
            JumpSpeed = _jumpSpeed;
            JumpHorizontalDampening = _jumpHorizontalDampening;
            VanishRange = _vanishRange;
            DodgeSpeed = _dodgeSpeed;
            DodgeDuration = _dodgeDuration;
            DodgeCooldown = _dodgeCooldown;
            KickAttackPower = _kickAttackPower;
            KickAttackRange = _kickAttackRange;
            KickAttackCooldown = _kickAttackCooldown;
            FireCooldown = _fireCooldown;
            IsGrounded = true;
            IsDodging = false;
        }

        public void IncrementDragonBallCount() => DragonBallCount++;
    }
}