using DragonBall.Utilities;

namespace DragonBall.Enemy
{
    public class DeathState : IState
    {
        private EnemyController enemyController;
        private EnemyStateMachine enemyStateMachine;

        public DeathState(EnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
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