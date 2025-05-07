using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class AttackState : IState
    {
        private BaseEnemyController baseEnemyController;
        private BaseEnemyModel baseEnemyModel;
        private EnemyStateMachine baseEnemyStateMachine;
        private Transform playerTransform;
        private EnemyScriptableObject enemyScriptableObject;

        public AttackState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
        {
            baseEnemyController = controllerToSet;
            baseEnemyStateMachine = stateMachineToSet;
            enemyScriptableObject = controllerToSet.EnemyData;
            baseEnemyModel = controllerToSet.BaseEnemyModel;

            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        public void OnStateEnter() => baseEnemyController.BaseEnemyView.SetMoving(false);

        public virtual void Update()
        {
            if (baseEnemyController.IsDead || baseEnemyController.BaseEnemyView.IsDying)
            {
                if (baseEnemyController.IsDead && !baseEnemyController.BaseEnemyView.IsDying)
                    baseEnemyStateMachine.ChangeState(EnemyStates.DEATH);
                return;
            }

            if (baseEnemyController.IsPlayerDead || playerTransform == null)
            {
                if (baseEnemyController.IsPlayerDead)
                    baseEnemyController.BaseEnemyView.StopMovement();

                baseEnemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            float distanceToPlayer = Vector2.Distance(baseEnemyController.BaseEnemyView.transform.position, playerTransform.position);

            if (distanceToPlayer > enemyScriptableObject.DetectionRange)
            {
                baseEnemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            if (distanceToPlayer > enemyScriptableObject.AttackRange && distanceToPlayer <= enemyScriptableObject.DetectionRange)
            {
                baseEnemyStateMachine.ChangeState(EnemyStates.RUNNING);
                return;
            }

            baseEnemyController.BaseEnemyView.FaceTarget(playerTransform.position);
            TryAttack();
        }

        private void TryAttack()
        {
            if (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead || baseEnemyController.BaseEnemyView.IsDying)
                return;

            if (Time.time < baseEnemyModel.lastAttackTime + enemyScriptableObject.AttackCooldown)
                return;

            if (!baseEnemyController.BaseEnemyView.IsAttacking)
            {
                baseEnemyController.BaseEnemyView.StartAttack();
                baseEnemyModel.lastAttackTime = Time.time;
            }
        }

        public void OnStateExit() { }
    }
}