using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class RunningState : IState
    {
        private EnemyController enemyController;
        private EnemyStateMachine stateMachine;
        private Transform playerTransform;
        private EnemyScriptableObject enemySO;

        public RunningState(EnemyController controller, EnemyStateMachine stateMachine)
        {
            this.enemyController = controller;
            this.stateMachine = stateMachine;
            this.enemySO = controller.EnemySO;
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        public void OnStateEnter()
        {
            enemyController.View.SetMoving(true);
        }

        public void Update()
        {
            if (playerTransform == null || enemyController.IsDead)
            {
                stateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            float distanceToPlayer = Vector2.Distance(enemyController.View.transform.position, playerTransform.position);

            // If player is no longer in detection range
            if (distanceToPlayer > enemySO.DetectionRange)
            {
                stateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            // If player is in attack range
            if (distanceToPlayer <= enemySO.AttackRange)
            {
                stateMachine.ChangeState(EnemyStates.ATTACK);
                return;
            }

            // Move towards player
            Vector2 direction = ((Vector2)playerTransform.position - (Vector2)enemyController.View.transform.position).normalized;
            enemyController.View.MoveInDirection(direction, enemySO.MoveSpeed);
        }

        public void OnStateExit()
        {
            // Nothing specific to clean up
        }
    }
}