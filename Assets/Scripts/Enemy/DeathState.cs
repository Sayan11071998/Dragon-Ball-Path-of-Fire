using DragonBall.Sound;

namespace DragonBall.Enemy
{
    public class DeathState : BaseState
    {
        public DeathState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
            : base(controllerToSet, stateMachineToSet) { }

        public override void OnStateEnter()
        {
            baseEnemyController.BaseEnemyView.StopMovement();
            baseEnemyController.BaseEnemyView.StartDeathAnimation();
            SoundManager.Instance.PlaySoundEffect(SoundType.KickTypeEnemyDeath);
        }

        public override void OnStateExit() => baseEnemyController.BaseEnemyView.ResetDeathState();
    }
}