using DragonBall.Bullet;
using DragonBall.Core;
using UnityEngine;

namespace DragonBall.Player
{
    public class SuperSaiyanState : BasePlayerState
    {
        public SuperSaiyanState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

        public override void OnStateEnter()
        {
            Debug.Log("Entering SUPER_SAIYAN state");
            playerModel.ApplySuperSaiyanBuffs();
        }

        public override void OnStateExit()
        {
            Debug.Log("Exiting SUPER_SAIYAN state");
            playerModel.RemoveSuperSaiyanBuffs();
        }

        protected override void HandleStateSpecificAbilities()
        {
            HandleDodge();
            HandleVanish();
            HandleKamehameha();
        }

        protected override void FireBullet()
        {
            Vector2 position = playerView.FireTransform.position;
            Vector2 direction = playerModel.IsFacingRight ? Vector2.right : Vector2.left;
            GameService.Instance.bulletService.FireBullet(BulletType.PlayerRegularPowerBall, position, direction);
        }
    }
}