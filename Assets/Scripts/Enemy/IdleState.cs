using UnityEngine;

namespace DragonBall.Enemy
{
    public class IdleState : BaseEnemyState
    {
        public IdleState(EnemyController controller, EnemyStateMachine stateMachine) : base(controller, stateMachine) { }

        public override void OnStateEnter()
        {
            Debug.Log("Enemy entered IDLE state");
            enemyView.UpdateMoveAnimation(false);
        }

        public override void Update()
        {
            // Check if player is within sensing range
            float distanceToPlayer = enemyController.GetDistanceToPlayer();

            if (distanceToPlayer <= enemyModel.SensingRange)
            {
                stateMachine.ChangeState(EnemyStateType.CHASE);
            }
        }

        public override void OnStateExit() { }
    }
}