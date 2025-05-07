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

            if (distanceToPlayer <= enemyScriptableObject.AttackRange - ATTACK_RANGE_BUFFER)
                enemyStateMachine.ChangeState(EnemyStates.ATTACK);
            else if (distanceToPlayer > enemyScriptableObject.DetectionRange)
                enemyStateMachine.ChangeState(EnemyStates.IDLE);

            if (!baseEnemyController.IsPlayerDead)
            {
                Vector2 direction = ((Vector2)playerTransform.position - (Vector2)baseEnemyController.BaseEnemyView.transform.position).normalized;
                baseEnemyController.BaseEnemyView.MoveInDirection(direction, enemyScriptableObject.MoveSpeed);
            }
        }

        public override void OnStateExit() => baseEnemyController.BaseEnemyView.SetMoving(false);
    }
}