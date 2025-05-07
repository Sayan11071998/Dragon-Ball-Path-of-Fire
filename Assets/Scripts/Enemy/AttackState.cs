using UnityEngine;

namespace DragonBall.Enemy
{
    public class AttackState : BaseState
    {
        protected BaseEnemyModel baseEnemyModel;

        private const float EXIT_ATTACK_BUFFER = 0.3f;

        public AttackState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
            : base(controllerToSet, stateMachineToSet)
        {
            baseEnemyModel = controllerToSet.BaseEnemyModel;
        }

        public override void OnStateEnter() => baseEnemyController.BaseEnemyView.SetMoving(false);

        protected override void StateSpecificUpdate()
        {
            float distanceToPlayer = GetDistanceToPlayer();

            if (distanceToPlayer > enemyScriptableObject.DetectionRange)
            {
                enemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            if (distanceToPlayer > (enemyScriptableObject.AttackRange + EXIT_ATTACK_BUFFER) &&
                distanceToPlayer <= enemyScriptableObject.DetectionRange)
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

        protected virtual bool CanAttack() => !baseEnemyController.IsPlayerDead && !baseEnemyController.IsDead && !baseEnemyController.BaseEnemyView.IsDying;

        protected virtual bool IsAttackCooldownComplete() => Time.time >= baseEnemyModel.lastAttackTime + enemyScriptableObject.AttackCooldown;

        protected virtual void ExecuteAttack()
        {
            baseEnemyController.BaseEnemyView.StartAttack();
            baseEnemyModel.lastAttackTime = Time.time;
        }
    }
}