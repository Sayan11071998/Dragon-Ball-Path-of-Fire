using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class IdleState : IState
    {
        private BaseEnemyController baseEnemyController;
        private EnemyStateMachine enemyStateMachine;
        private Transform playerTransform;
        private EnemyScriptableObject enemyScriptableObject;

        public IdleState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
        {
            baseEnemyController = controllerToSet;
            enemyStateMachine = stateMachineToSet;
            enemyScriptableObject = controllerToSet.EnemyData;

            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        public void OnStateEnter() => baseEnemyController.BaseEnemyView.SetMoving(false);

        public void Update()
        {
            if (playerTransform == null || baseEnemyController.IsDead || baseEnemyController.IsPlayerDead) return;

            float distanceToPlayer = Vector2.Distance(baseEnemyController.BaseEnemyView.transform.position, playerTransform.position);

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