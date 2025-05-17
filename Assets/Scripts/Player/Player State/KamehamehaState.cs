using UnityEngine;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Player.PlayerData;
using DragonBall.Sound.SoundUtilities;
using DragonBall.Sound.SoundData;

namespace DragonBall.Player.PlayerStates
{
    public class KamehamehaState : PlayerStateBase
    {
        private bool kamehamehaAnimationComplete = false;
        private float kamehamehaStartTime;
        private float kamehamehaAnimationDuration;
        private bool hasFiredKamehameha = false;
        private bool wasFlying = false;

        public KamehamehaState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine)
        {
            kamehamehaAnimationDuration = playerView.KamehamehaAnimationClip.length;
        }

        public override void OnStateEnter()
        {
            kamehamehaAnimationComplete = false;
            hasFiredKamehameha = false;
            kamehamehaStartTime = Time.time;

            wasFlying = playerModel.IsFlying;

            if (playerModel.UseStaminaForKamehameha())
            {
                playerView.PlayKamehamehaAnimation();
                SoundManager.Instance.PlaySoundEffect(SoundType.Kamekameha);
            }
            else
            {
                kamehamehaAnimationComplete = true;
            }

            playerView.ResetKamehameha();
        }

        public override void Update()
        {
            base.Update();

            if (!kamehamehaAnimationComplete && !hasFiredKamehameha && Time.time >= kamehamehaStartTime + kamehamehaAnimationDuration)
            {
                if (playerModel.HasEnoughStaminaForKamehameha)
                {
                    playerController.FireKamehameha();
                    hasFiredKamehameha = true;
                }
                kamehamehaAnimationComplete = true;
            }

            if (kamehamehaAnimationComplete)
            {
                if (wasFlying && playerModel.IsSuperSaiyan())
                    stateMachine.ChangeState(PlayerState.Fly);
                else if (ShouldTransitionToMove())
                    stateMachine.ChangeState(PlayerState.Run);
                else
                    stateMachine.ChangeState(PlayerState.Idle);
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            hasFiredKamehameha = false;
        }
    }
}