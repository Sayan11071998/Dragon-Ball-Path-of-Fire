namespace DragonBall.Player
{
    public class PlayerModel
    {
        public int Health { get; private set; }
        public float MoveSpeed { get; private set; }
        public float JumpForce { get; private set; }
        public bool IsGrounded { get; set; }
        public bool IsFacingRight { get; set; } = true;

        public PlayerModel(int _Health, float _moveSpeed, float _jumpForce)
        {
            Health = _Health;
            MoveSpeed = _moveSpeed;
            JumpForce = _jumpForce;
            IsGrounded = true;
        }
    }
}