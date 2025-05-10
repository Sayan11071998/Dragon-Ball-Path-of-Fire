namespace DragonBall.Enemy
{
    public class IdleState : BaseState
    {
        public IdleState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
            : base(controllerToSet, stateMachineToSet) { }

        public override void OnStateEnter() => baseEnemyController.BaseEnemyView.SetMoving(false);

        protected override void StateSpecificUpdate()
        {
            if (playerTransform == null || baseEnemyController.IsDead || baseEnemyController.IsPlayerDead)
                return;

            float distanceToPlayer = GetDistanceToPlayer();

            if (distanceToPlayer < enemyScriptableObject.DetectionRange && HasLineOfSightToPlayer())
            {
                if (distanceToPlayer < enemyScriptableObject.AttackRange)
                    enemyStateMachine.ChangeState(EnemyStates.ATTACK);
                else
                    enemyStateMachine.ChangeState(EnemyStates.RUNNING);
            }
        }
    }
}