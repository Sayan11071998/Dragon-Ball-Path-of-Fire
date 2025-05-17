namespace DragonBall.Player.PlayerStates
{
    using UnityEngine;
    using DragonBall.Player.PlayerMVC;
    using DragonBall.Player.PlayerData;
    using DragonBall.Player.PlayerUtilities;
    
    public class RunState : BasePlayerState
    {
        public RunState(PlayerController controller, PlayerStateManager manager) 
            : base(controller, manager) { }
            
        public override void Enter()
        {
            playerView.UpdateRunAnimation(true);
        }
        
        public override void Update()
        {
            // Handle movement
            float moveInput = playerView.MoveInput;
            Vector2 velocity = GetCurrentVelocity();
            velocity.x = moveInput * playerModel.MoveSpeed;
            SetVelocity(velocity);
            
            // Handle character flipping
            if (moveInput > 0 && !IsFacingRight())
            {
                FlipCharacter(true);
            }
            else if (moveInput < 0 && IsFacingRight())
            {
                FlipCharacter(false);
            }
            
            // Check transitions
            if (Mathf.Abs(moveInput) < 0.1f)
            {
                stateManager.ChangeState(PlayerState.Idle);
                return;
            }
            
            if (playerView.JumpInput && IsGrounded())
            {
                stateManager.ChangeState(PlayerState.Jump);
                return;
            }
            
            if (!IsGrounded())
            {
                stateManager.ChangeState(PlayerState.Fall);
                return;
            }
            
            if (playerView.DodgeInput && stateManager.CanDodge())
            {
                stateManager.ChangeState(PlayerState.Dodge);
                return;
            }
            
            if (playerView.FlyInput && stateManager.CanFly())
            {
                stateManager.ChangeState(PlayerState.Fly);
                return;
            }
            
            if (playerView.KickInput && !playerModel.IsKickOnCooldown)
            {
                stateManager.ChangeState(PlayerState.Kick);
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
            playerView.UpdateRunAnimation(false);
        }
        
        public override PlayerState GetStateType() => PlayerState.Run;
    }
}