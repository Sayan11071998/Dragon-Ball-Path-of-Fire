using UnityEngine;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Player.PlayerData;
using DragonBall.Sound.SoundUtilities;
using DragonBall.Sound.SoundData;

namespace DragonBall.Player.PlayerStates
{
    public class FlyState : PlayerStateBase
    {
        private const float MaxVerticalSpeed = 10f;
        private const float VerticalDamping = 0.92f;
        private float lastFlightEffectTime;
        private readonly float flightEffectInterval = 0.5f;

        public FlyState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            if (!playerModel.IsSuperSaiyan())
            {
                stateMachine.ChangeState(PlayerState.Idle);
                return;
            }

            playerModel.IsFlying = true;
            playerView.Rigidbody.linearVelocity = Vector2.zero;
            playerView.ResetMovementDirection();
            playerView.UpdateFlightAnimation(true);
            playerView.Rigidbody.gravityScale = 0f;
            playerView.StartFlightSound();
            playerView.ResetFlyInput();

            lastFlightEffectTime = Time.time;
        }

        public override void Update()
        {
            if (playerView.FireInput && !playerModel.IsFireOnCooldown)
            {
                stateMachine.ChangeState(PlayerState.Fire);
                return;
            }

            if (playerView.KamehamehaInput && playerModel.HasEnoughStaminaForKamehameha)
            {
                stateMachine.ChangeState(PlayerState.Kamehameha);
                return;
            }

            if (playerView.VanishInput)
            {
                stateMachine.ChangeState(PlayerState.Vanish);
                return;
            }

            base.Update();

            HandleFlightMovement();
            PlayFlightEffects();

            if (!playerModel.IsSuperSaiyan())
                ExitFlyState();
        }

        public override void OnStateExit()
        {
            playerView.UpdateFlightAnimation(false);
            playerView.Rigidbody.gravityScale = 1f;
            playerView.StopFlightSound();

            PlayerState nextState = stateMachine.GetCurrentPlayerState();

            if (nextState != PlayerState.Fire && nextState != PlayerState.Kamehameha && nextState != PlayerState.Vanish)
                playerModel.IsFlying = false;

            base.OnStateExit();
        }

        private void HandleFlightMovement()
        {
            Vector2 movementDirection = playerView.MovementDirection;
            Vector2 velocity = playerView.Rigidbody.linearVelocity;

            float horizontalVelocity = movementDirection.x * playerModel.FlySpeed;
            float verticalVelocity = movementDirection.y * playerModel.FlySpeed;

            velocity.x = horizontalVelocity;
            velocity.y = Mathf.Lerp(velocity.y, verticalVelocity, Time.deltaTime * 5f);
            velocity.y = Mathf.Clamp(velocity.y, -MaxVerticalSpeed, MaxVerticalSpeed);

            if (Mathf.Abs(movementDirection.y) < 0.1f)
                velocity.y *= VerticalDamping;

            playerView.Rigidbody.linearVelocity = velocity;
        }

        private void PlayFlightEffects()
        {
            Vector2 velocity = playerView.Rigidbody.linearVelocity;
            if ((velocity.sqrMagnitude > 1f) && Time.time >= lastFlightEffectTime + flightEffectInterval)
            {
                lastFlightEffectTime = Time.time;
                SoundManager.Instance.PlaySoundEffect(SoundType.GokuFly);
            }
        }

        private void ExitFlyState() => stateMachine.ChangeState(PlayerState.Idle);
    }
}