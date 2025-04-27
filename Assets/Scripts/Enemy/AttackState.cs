using DragonBall.Core;
using DragonBall.Player;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class AttackState : BaseEnemyState
    {
        public AttackState(EnemyController controller, EnemyStateMachine stateMachine) : base(controller, stateMachine) { }

        public override void OnStateEnter()
        {
            Debug.Log("Enemy entered ATTACK state");
            enemyView.UpdateMoveAnimation(false);
        }

        public override void Update()
        {
            float distanceToPlayer = enemyController.GetDistanceToPlayer();
            Vector2 directionToPlayer = enemyController.GetDirectionToPlayer();

            // Face the player
            FacePlayer(directionToPlayer);

            if (distanceToPlayer > enemyModel.AttackRange)
            {
                stateMachine.ChangeState(EnemyStateType.CHASE);
                return;
            }

            if (!enemyModel.IsAttackOnCooldown)
            {
                PerformAttack();
                enemyModel.LastAttackTime = Time.time;
            }
        }

        private void PerformAttack()
        {
            enemyView.PlayAttackAnimation();

            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                enemyView.AttackTransform.position,
                enemyModel.AttackRange,
                enemyController.GetDirectionToPlayer(),
                0f,
                enemyView.AttackableLayer);

            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if (hit.collider.TryGetComponent<IDamageable>(out var target))
                    {
                        target.Damage(enemyModel.AttackPower);
                        Debug.Log($"Enemy attacked player for {enemyModel.AttackPower} damage!");
                    }
                }
            }
        }

        public override void OnStateExit() { }
    }
}