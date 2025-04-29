using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class RunningState : IState
    {
        private BaseEnemyController baseEnemyController;
        private EnemyStateMachine enemyStateMachine;
        private Transform playerTransform;
        private EnemyScriptableObject enemyScriptableObject;

        public RunningState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
        {
            baseEnemyController = controllerToSet;
            enemyStateMachine = stateMachineToSet;
            enemyScriptableObject = controllerToSet.EnemyData;

            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        public void OnStateEnter()
        {
            if (!baseEnemyController.IsPlayerDead)
                baseEnemyController.BaseEnemyView.SetMoving(true);
        }

        public void Update()
        {
            if (baseEnemyController.IsPlayerDead)
            {
                baseEnemyController.BaseEnemyView.StopMovement();
                enemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            if (playerTransform == null || baseEnemyController.IsDead)
            {
                enemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            float distanceToPlayer = Vector2.Distance(baseEnemyController.BaseEnemyView.transform.position, playerTransform.position);

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

            if (!baseEnemyController.IsPlayerDead)
            {
                Vector2 direction = ((Vector2)playerTransform.position - (Vector2)baseEnemyController.BaseEnemyView.transform.position).normalized;
                baseEnemyController.BaseEnemyView.MoveInDirection(direction, enemyScriptableObject.MoveSpeed);
            }
        }

        public void OnStateExit() => baseEnemyController.BaseEnemyView.SetMoving(false);
    }
}