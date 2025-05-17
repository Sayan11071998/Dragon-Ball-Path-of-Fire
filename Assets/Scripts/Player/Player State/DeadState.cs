namespace DragonBall.Player.PlayerStates
{
    using System.Collections;
    using UnityEngine;
    using DragonBall.Player.PlayerMVC;
    using DragonBall.Player.PlayerData;
    using DragonBall.Player.PlayerUtilities;
    using DragonBall.Sound.SoundUtilities;
    using DragonBall.Sound.SoundData;
    using DragonBall.Core;
    
    public class DeadState : BasePlayerState
    {
        public DeadState(PlayerController controller, PlayerStateManager manager) 
            : base(controller, manager) { }
            
        public override void Enter()
        {
            playerView.DisableInput();
            playerView.StopPlayerMovement();
            playerView.PlayDeathAnimation();
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuDeath);
            
            if (playerModel.IsFlying)
                playerView.StopFlightSound();
                
            playerView.StartCoroutine(DeathSequence());
        }
        
        private IEnumerator DeathSequence()
        {
            float directionX = playerView.transform.localScale.x > 0 ? -1 : 1;
            
            playerView.Rigidbody.linearVelocity = Vector2.zero;
            playerView.Rigidbody.AddForce(new Vector2(directionX * 5f, 2f), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.3f);
            
            yield return new WaitForSeconds(1.5f); // Adjust based on death animation length
            
            GameService.Instance.uiService.ShowGameOver();
            playerView.gameObject.SetActive(false);
        }
        
        public override void Update()
        {
            // No state transitions from dead
        }
        
        public override PlayerState GetStateType() => PlayerState.Dead;
    }
}