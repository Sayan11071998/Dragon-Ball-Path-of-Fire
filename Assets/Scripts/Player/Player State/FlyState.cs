namespace DragonBall.Player.PlayerStates
{
    using UnityEngine;
    using DragonBall.Player.PlayerMVC;
    using DragonBall.Player.PlayerData;
    using DragonBall.Player.PlayerUtilities;
    
    public class FlyState : BasePlayerState
    {
        public FlyState(PlayerController controller, PlayerStateManager manager) 
            : base(controller, manager) { }
            
        public override void Enter()
        {
            playerModel.IsFlying = true;
            playerView.UpdateFlightAnimation(true);
            playerView.Rigidbody.gravityScale = 0f;
            playerView.StartFlightSound();
            playerView.ResetFlyInput();
        }
        
        public override void Update()
        {
            // Handle flying movement
            Vector2 moveDirection = playerView.MovementDirection;
            Vector2 velocity = GetCurrentVelocity();
            
            if (Mathf.Abs(moveDirection.x) > 0.1f)
                velocity.x = moveDirection.x * playerModel.MoveSpeed;
            else
                velocity.x = 0;
                
            if (Mathf.Abs(moveDirection.y) > 0.1f)
                velocity.y = moveDirection.y * playerModel.FlySpeed;
            else
                velocity.y = 0;
                
            SetVelocity(velocity);
            
            // Handle character direction
            if (moveDirection.x > 0 && !IsFacingRight())
            {
                FlipCharacter(true);
            }
            else if (moveDirection.x < 0 && IsFacingRight())
            {
                FlipCharacter(false);
            }
            
            // Check transitions
            if (playerView.FlyInput)
            {
                stateManager.ChangeState(PlayerState.Fall);
                return;
            }
            
            if (playerView.VanishInput && stateManager.CanVanish())
            {
                stateManager.ChangeState(PlayerState.Vanish);
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
            
            if (playerView.KamehamehaInput && HasEnoughStaminaForKamehameha() && stateManager.CanUseKamehameha())
            {
                stateManager.ChangeState(PlayerState.Kamehameha);
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
            playerModel.IsFlying = false;
            playerView.UpdateFlightAnimation(false);
            playerView.Rigidbody.gravityScale = 1f;
            playerView.StopFlightSound();
            playerView.ResetMovementDirection();
        }
        
        public override PlayerState GetStateType() => PlayerState.Fly;
    }
}