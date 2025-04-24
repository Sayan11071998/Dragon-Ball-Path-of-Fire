namespace DragonBall.Player
{
    public class PlayerModel
    {
        public int Health { get; private set; }
        public float MoveSpeed { get; private set; }
        public float JumpSpeed { get; private set; }
        public float VanishRange { get; private set; }
        public bool IsGrounded { get; set; }
        public bool IsFacingRight { get; set; } = true;
        public int JumpCount { get; set; } = 0;

        public PlayerModel(int _Health, float _moveSpeed, float _jumpSpeed, float _vanishRange)
        {
            Health = _Health;
            MoveSpeed = _moveSpeed;
            JumpSpeed = _jumpSpeed;
            VanishRange = _vanishRange;
            IsGrounded = true;
        }
    }
}