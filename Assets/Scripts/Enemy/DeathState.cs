using DragonBall.Utilities;

namespace DragonBall.Enemy
{
    public class DeathState : IState
    {
        private BaseEnemyController enemyController;
        private EnemyStateMachine enemyStateMachine;

        public DeathState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
        {
            enemyController = controllerToSet;
            enemyStateMachine = stateMachineToSet;
        }

        public void OnStateEnter()
        {
            enemyController.EnemyView.StopMovement();
            enemyController.EnemyView.StartDeathAnimation();
        }

        public void Update() { }

        public void OnStateExit() => enemyController.EnemyView.ResetDeathState();
    }
}