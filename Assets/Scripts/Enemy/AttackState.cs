using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class AttackState : IState
    {
        private BaseEnemyController enemyController;
        private EnemyStateMachine enemyStateMachine;
        private Transform playerTransform;
        private EnemyScriptableObject enemyScriptableObject;
        private BaseEnemyModel enemyModel;

        public AttackState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
        {
            enemyController = controllerToSet;
            enemyStateMachine = stateMachineToSet;
            enemyScriptableObject = controllerToSet.EnemyData;
            enemyModel = controllerToSet.EnemyModel;

            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        public void OnStateEnter() => enemyController.EnemyView.SetMoving(false);

        public void Update()
        {
            if (enemyController.isPlayerDead || playerTransform == null || enemyController.IsDead)
            {
                if (enemyController.isPlayerDead)
                    enemyController.EnemyView.StopMovement();

                enemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            float distanceToPlayer = Vector2.Distance(enemyController.EnemyView.transform.position, playerTransform.position);

            if (distanceToPlayer > enemyScriptableObject.DetectionRange)
            {
                enemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            if (distanceToPlayer > enemyScriptableObject.AttackRange && distanceToPlayer <= enemyScriptableObject.DetectionRange)
            {
                enemyStateMachine.ChangeState(EnemyStates.RUNNING);
                return;
            }

            enemyController.EnemyView.FaceTarget(playerTransform.position);
            TryAttack();
        }

        private void TryAttack()
        {
            if (enemyController.isPlayerDead) return;
            if (Time.time < enemyModel.lastAttackTime + enemyScriptableObject.AttackCooldown) return;

            if (!enemyController.EnemyView.IsAttacking)
            {
                enemyController.EnemyView.StartAttack();
                enemyModel.lastAttackTime = Time.time;
            }
        }

        public void OnStateExit() { }
    }
}