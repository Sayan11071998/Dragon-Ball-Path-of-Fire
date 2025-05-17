using UnityEngine;
using System.Collections;
using DragonBall.Core;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Sound.SoundData;
using DragonBall.Sound.SoundUtilities;

namespace DragonBall.Player.PlayerStates
{
    public class DeadState : BasePlayerState
    {
        public DeadState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
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

            yield return new WaitForSeconds(1.5f);

            GameService.Instance.uiService.ShowGameOver();
            playerView.gameObject.SetActive(false);
        }

        public override void Update() { }

        public override void OnStateExit() { }

        public override PlayerState GetStateType() => PlayerState.Dead;
    }
}