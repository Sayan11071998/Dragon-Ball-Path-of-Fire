using UnityEngine;

namespace DragonBall.Enemy
{
    public class ChaseState : BaseEnemyState
    {
        public ChaseState(EnemyController controller, EnemyStateMachine stateMachine) : base(controller, stateMachine) { }

        public override void OnStateEnter()
        {
            Debug.Log("Enemy entered CHASE state");
            enemyView.UpdateMoveAnimation(true);
        }

        public override void Update()
        {
            float distanceToPlayer = enemyController.GetDistanceToPlayer();
            Vector2 directionToPlayer = enemyController.GetDirectionToPlayer();

            // Face the player
            FacePlayer(directionToPlayer);

            if (distanceToPlayer <= enemyModel.AttackRange)
            {
                stateMachine.ChangeState(EnemyStateType.ATTACK);
                return;
            }

            if (distanceToPlayer > enemyModel.SensingRange)
            {
                stateMachine.ChangeState(EnemyStateType.IDLE);
                return;
            }

            // Move towards player
            if (distanceToPlayer > enemyModel.StoppingDistance)
            {
                enemyView.SetVelocity(directionToPlayer * enemyModel.MoveSpeed);
            }
            else
            {
                enemyView.SetVelocity(Vector2.zero);
            }
        }

        public override void OnStateExit()
        {
            enemyView.SetVelocity(Vector2.zero);
        }
    }
}