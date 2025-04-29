using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class IdleState : IState
    {
        private BaseEnemyController enemyController;
        private EnemyStateMachine enemyStateMachine;
        private Transform playerTransform;
        private EnemyScriptableObject enemyScriptableObject;

        public IdleState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
        {
            enemyController = controllerToSet;
            enemyStateMachine = stateMachineToSet;
            enemyScriptableObject = controllerToSet.EnemyData;

            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        public void OnStateEnter() => enemyController.EnemyView.SetMoving(false);

        public void Update()
        {
            if (playerTransform == null || enemyController.IsDead || enemyController.isPlayerDead) return;

            float distanceToPlayer = Vector2.Distance(enemyController.EnemyView.transform.position, playerTransform.position);

            if (distanceToPlayer <= enemyScriptableObject.DetectionRange)
            {
                if (distanceToPlayer <= enemyScriptableObject.AttackRange)
                    enemyStateMachine.ChangeState(EnemyStates.ATTACK);
                else
                    enemyStateMachine.ChangeState(EnemyStates.RUNNING);
            }
        }

        public void OnStateExit() { }
    }
}