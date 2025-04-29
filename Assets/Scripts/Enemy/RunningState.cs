using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class RunningState : IState
    {
        private BaseEnemyController enemyController;
        private EnemyStateMachine enemyStateMachine;
        private Transform playerTransform;
        private EnemyScriptableObject enemyScriptableObject;

        public RunningState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
        {
            enemyController = controllerToSet;
            enemyStateMachine = stateMachineToSet;
            enemyScriptableObject = controllerToSet.EnemyData;

            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        public void OnStateEnter()
        {
            if (!enemyController.isPlayerDead)
                enemyController.EnemyView.SetMoving(true);
        }

        public void Update()
        {
            if (enemyController.isPlayerDead)
            {
                enemyController.EnemyView.StopMovement();
                enemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            if (playerTransform == null || enemyController.IsDead)
            {
                enemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            float distanceToPlayer = Vector2.Distance(enemyController.EnemyView.transform.position, playerTransform.position);

            if (distanceToPlayer > enemyScriptableObject.DetectionRange)
            {
                enemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            if (distanceToPlayer <= enemyScriptableObject.AttackRange)
            {
                enemyStateMachine.ChangeState(EnemyStates.ATTACK);
                return;
            }

            if (!enemyController.isPlayerDead)
            {
                Vector2 direction = ((Vector2)playerTransform.position - (Vector2)enemyController.EnemyView.transform.position).normalized;
                enemyController.EnemyView.MoveInDirection(direction, enemyScriptableObject.MoveSpeed);
            }
        }

        public void OnStateExit() => enemyController.EnemyView.SetMoving(false);
    }
}