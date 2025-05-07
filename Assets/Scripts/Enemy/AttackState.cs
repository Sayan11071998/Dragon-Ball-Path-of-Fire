using UnityEngine;

namespace DragonBall.Enemy
{
    public class AttackState : BaseState
    {
        protected BaseEnemyModel baseEnemyModel;

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

            if (distanceToPlayer > enemyScriptableObject.AttackRange && distanceToPlayer <= enemyScriptableObject.DetectionRange)
            {
                enemyStateMachine.ChangeState(EnemyStates.RUNNING);
                return;
            }

            baseEnemyController.BaseEnemyView.FaceTarget(playerTransform.position);
            TryAttack();
        }

        protected virtual void TryAttack()
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
    }
}