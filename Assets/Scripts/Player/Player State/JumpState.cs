namespace DragonBall.Player.PlayerStates
{
    using UnityEngine;
    using DragonBall.Player.PlayerMVC;
    using DragonBall.Player.PlayerData;
    using DragonBall.Player.PlayerUtilities;
    using DragonBall.Sound.SoundUtilities;
    using DragonBall.Sound.SoundData;
    
    public class JumpState : BasePlayerState
    {
        public JumpState(PlayerController controller, PlayerStateManager manager) 
            : base(controller, manager) { }
            
        public override void Enter()
        {
            playerView.UpdateJumpAnimation(true);
            
            Vector2 velocity = GetCurrentVelocity();
            velocity.y = playerModel.JumpSpeed;
            velocity.x *= playerModel.JumpHorizontalDampening;
            SetVelocity(velocity);
            
            playerModel.JumpCount++;
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuJump);
            playerView.ResetJumpInput();
        }
        
        public override void Update()
        {
            // Handle horizontal movement during jump
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
            if (GetCurrentVelocity().y < 0)
            {
                stateManager.ChangeState(PlayerState.Fall);
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
        
        public override PlayerState GetStateType() => PlayerState.Jump;
    }
}