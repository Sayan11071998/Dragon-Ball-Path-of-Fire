using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class DeathState : IState
    {
        private EnemyController enemyController;
        private EnemyStateMachine stateMachine;

        public DeathState(EnemyController controller, EnemyStateMachine stateMachine)
        {
            this.enemyController = controller;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            // Stop movement
            enemyController.View.StopMovement();

            // Start death animation
            enemyController.View.StartDeathAnimation();
        }

        public void Update()
        {
            // Death state doesn't require updates - animation triggers callbacks
        }

        public void OnStateExit()
        {
            // Reset death state for future reuse
            enemyController.View.ResetDeathState();
        }
    }
}