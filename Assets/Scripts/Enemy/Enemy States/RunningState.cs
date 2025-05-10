using UnityEngine;

namespace DragonBall.Enemy
{
    public class RunningState : BaseState
    {
        private const float ATTACK_RANGE_BUFFER = 0.2f;

        public RunningState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
            : base(controllerToSet, stateMachineToSet) { }

        public override void OnStateEnter()
        {
            if (!baseEnemyController.IsPlayerDead)
                baseEnemyController.BaseEnemyView.SetMoving(true);
        }

        protected override void StateSpecificUpdate()
        {
            if (playerTransform == null || baseEnemyController.IsDead)
            {
                enemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            float distanceToPlayer = GetDistanceToPlayer();
            bool canSeePlayer = HasLineOfSightToPlayer();

            if (distanceToPlayer > enemyScriptableObject.DetectionRange || !canSeePlayer)
            {
                enemyStateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            if (distanceToPlayer <= enemyScriptableObject.AttackRange - ATTACK_RANGE_BUFFER && canSeePlayer)
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

        public override void OnStateExit() => baseEnemyController.BaseEnemyView.SetMoving(false);
    }
}