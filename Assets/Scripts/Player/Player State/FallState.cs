namespace DragonBall.Player.PlayerStates
{
    using UnityEngine;
    using DragonBall.Player.PlayerMVC;
    using DragonBall.Player.PlayerData;
    using DragonBall.Player.PlayerUtilities;
    
    public class FallState : BasePlayerState
    {
        public FallState(PlayerController controller, PlayerStateManager manager) 
            : base(controller, manager) { }
            
        public override void Enter()
        {
            playerView.UpdateJumpAnimation(true);
        }
        
        public override void Update()
        {
            // Handle horizontal movement during fall
            float moveInput = playerView.MoveInput;
            Vector2 velocity = GetCurrentVelocity();
            velocity.x = moveInput * playerModel.MoveSpeed;
            SetVelocity(velocity);
            
            // Handle character direction
            if (moveInput > 0 && !IsFacingRight())
            {
                FlipCharacter(true);
            }
            else if (moveInput < 0 && IsFacingRight())
            {
                FlipCharacter(false);
            }
            
            // Check transitions
            if (IsGrounded())
            {
                stateManager.ChangeState(Mathf.Abs(moveInput) > 0.1f ? PlayerState.Run : PlayerState.Idle);
                return;
            }
            
            if (playerView.JumpInput && playerModel.JumpCount < 1)
            {
                // Double jump
                Vector2 doubleJumpVelocity = GetCurrentVelocity();
                doubleJumpVelocity.y = playerModel.JumpSpeed;
                doubleJumpVelocity.x *= playerModel.JumpHorizontalDampening;
                SetVelocity(doubleJumpVelocity);
                
                playerModel.JumpCount++;
                return;
            }
            
            if (playerView.FlyInput && stateManager.CanFly())
            {
                stateManager.ChangeState(PlayerState.Fly);
                return;
            }
            
            if (playerView.VanishInput && stateManager.CanVanish())
            {
                stateManager.ChangeState(PlayerState.Vanish);
                return;
            }
            
            if (playerView.FireInput && !playerModel.IsFireOnCooldown)
            {
                stateManager.ChangeState(PlayerState.Fire);
                return;
            }
            
            if (playerModel.IsDead)
            {
                stateManager.ChangeState(PlayerState.Dead);
                return;
            }
        }
        
        public override void Exit()
        {
            playerView.UpdateJumpAnimation(false);
        }
        
        public override PlayerState GetStateType() => PlayerState.Fall;
    }
}