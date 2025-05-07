using UnityEngine;

namespace DragonBall.Enemy
{
    public class RunningState : BaseState
    {
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

        public override void OnStateExit() => baseEnemyController.BaseEnemyView.SetMoving(false);
    }
}