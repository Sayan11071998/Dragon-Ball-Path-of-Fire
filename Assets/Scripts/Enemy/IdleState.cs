using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class IdleState : IState
    {
        private EnemyController enemyController;
        private EnemyStateMachine stateMachine;
        private Transform playerTransform;
        private EnemyScriptableObject enemySO;

        public IdleState(EnemyController controller, EnemyStateMachine stateMachine)
        {
            this.enemyController = controller;
            this.stateMachine = stateMachine;
            this.enemySO = controller.EnemySO;
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        public void OnStateEnter()
        {
            enemyController.View.SetMoving(false);
        }

        public void Update()
        {
            if (playerTransform == null || enemyController.IsDead)
                return;

            float distanceToPlayer = Vector2.Distance(enemyController.View.transform.position, playerTransform.position);

            // Check if player is detected
            if (distanceToPlayer <= enemySO.DetectionRange)
            {
                // Check if in attack range
                if (distanceToPlayer <= enemySO.AttackRange)
                {
                    stateMachine.ChangeState(EnemyStates.ATTACK);
                }
                else
                {
                    stateMachine.ChangeState(EnemyStates.RUNNING);
                }
            }
        }

        public void OnStateExit()
        {
            // Nothing to clean up
        }
    }
}