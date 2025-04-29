using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class AttackState : IState
    {
        private EnemyController enemyController;
        private EnemyStateMachine stateMachine;
        private Transform playerTransform;
        private EnemyScriptableObject enemySO;
        private EnemyModel model;

        public AttackState(EnemyController controller, EnemyStateMachine stateMachine)
        {
            this.enemyController = controller;
            this.stateMachine = stateMachine;
            this.enemySO = controller.EnemySO;
            this.model = controller.Model;
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        public void OnStateEnter()
        {
            enemyController.View.SetMoving(false);
        }

        public void Update()
        {
            if (playerTransform == null || enemyController.IsDead)
            {
                stateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            float distanceToPlayer = Vector2.Distance(enemyController.View.transform.position, playerTransform.position);

            // If player is no longer in detection range
            if (distanceToPlayer > enemySO.DetectionRange)
            {
                stateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            // If player is not in attack range but still detected
            if (distanceToPlayer > enemySO.AttackRange && distanceToPlayer <= enemySO.DetectionRange)
            {
                stateMachine.ChangeState(EnemyStates.RUNNING);
                return;
            }

            // Face the player
            enemyController.View.FaceTarget(playerTransform.position);

            // Try to attack
            TryAttack();
        }

        private void TryAttack()
        {
            if (Time.time < model.lastAttackTime + enemySO.AttackCooldown)
                return;

            if (!enemyController.View.IsAttacking)
            {
                enemyController.View.StartAttack();
                model.lastAttackTime = Time.time;
            }
        }

        public void OnStateExit()
        {
            // Nothing specific to clean up
        }
    }
}