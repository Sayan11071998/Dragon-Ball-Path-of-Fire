namespace DragonBall.Player
{
    public class PlayerModel
    {
        public int Health { get; private set; }
        public float MoveSpeed { get; private set; }
        public float JumpSpeed { get; private set; }
        public bool IsGrounded { get; set; }
        public bool IsFacingRight { get; set; } = true;

        public PlayerModel(int _Health, float _moveSpeed, float _jumpSpeed)
        {
            Health = _Health;
            MoveSpeed = _moveSpeed;
            JumpSpeed = _jumpSpeed;
            IsGrounded = true;
        }
    }
}