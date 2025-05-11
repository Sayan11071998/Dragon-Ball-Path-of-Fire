using DragonBall.Enemy.EnemyData;
using DragonBall.Enemy.EnemyUtilities;
using DragonBall.Enemy.ParentMVC;
using UnityEngine;

namespace DragonBall.Enemy.EnemyState
{
    public class AttackState : BaseState
    {
        protected BaseEnemyModel baseEnemyModel;

        private float exitAttackBuffer;

        public AttackState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
            : base(controllerToSet, stateMachineToSet)
        {
            baseEnemyModel = controllerToSet.BaseEnemyModel;
            exitAttackBuffer = controllerToSet.EnemyData.ExitAttackBuffer;
        }

        public override void OnStateEnter() => baseEnemyController.BaseEnemyView.SetMoving(false);

        protected override void StateSpecificUpdate()
        {
            float distanceToPlayer = GetDistanceToPlayer();
            bool canSeePlayer = HasLineOfSightToPlayer();

            if (distanceToPlayer > enemyScriptableObject.DetectionRange || !canSeePlayer)
            {
                enemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            if (distanceToPlayer > (enemyScriptableObject.AttackRange + exitAttackBuffer) &&
                distanceToPlayer <= enemyScriptableObject.DetectionRange && canSeePlayer)
            {
                enemyStateMachine.ChangeState(EnemyStates.RUNNING);
                return;
            }

            baseEnemyController.BaseEnemyView.FaceTarget(playerTransform.position);
            TryAttack();
        }

        protected virtual void TryAttack()
        {
            if (!CanAttack()) return;
            if (!IsAttackCooldownComplete()) return;

            if (!baseEnemyController.BaseEnemyView.IsAttacking)
                ExecuteAttack();
        }

        protected virtual bool CanAttack() => !baseEnemyController.IsPlayerDead && !baseEnemyController.IsDead && !baseEnemyController.BaseEnemyView.IsDying && HasLineOfSightToPlayer();

        protected virtual bool IsAttackCooldownComplete() => Time.time >= baseEnemyModel.lastAttackTime + baseEnemyModel.AttackCooldown;

        protected virtual void ExecuteAttack()
        {
            baseEnemyController.BaseEnemyView.StartAttack();
            baseEnemyModel.lastAttackTime = Time.time;
        }
    }
}