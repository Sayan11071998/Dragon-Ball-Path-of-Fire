using DragonBall.Enemy.EnemyUtilities;
using DragonBall.Enemy.ParentMVC;
using DragonBall.Sound.SoundData;
using DragonBall.Sound.SoundUtilities;

namespace DragonBall.Enemy.EnemyState
{
    public class DeathState : BaseState
    {
        public DeathState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
            : base(controllerToSet, stateMachineToSet) { }

        public override void OnStateEnter()
        {
            baseEnemyController.BaseEnemyView.StopMovement();
            baseEnemyController.BaseEnemyView.StartDeathAnimation();
            SoundManager.Instance.PlaySoundEffect(SoundType.EnemyDeath);
        }

        public override void OnStateExit() => baseEnemyController.BaseEnemyView.ResetDeathState();
    }
}