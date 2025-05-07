using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class FinalBossAttackState : AttackState
    {
        private FinalBossTypeEnemyController finalBossController;
        private FinalBossTypeEnemyModel finalBossModel;
        private FinalBossTypeEnemyView finalBossView;
        private Transform playerTransform;
        private FinalBossTypeEnemyScriptableObject bossData;
        private EnemyStateMachine stateMachine;

        public FinalBossAttackState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
            : base(controllerToSet, stateMachineToSet)
        {
            finalBossController = controllerToSet as FinalBossTypeEnemyController;
            finalBossModel = finalBossController.BaseEnemyModel as FinalBossTypeEnemyModel;
            finalBossView = finalBossController.BaseEnemyView as FinalBossTypeEnemyView;
            bossData = controllerToSet.EnemyData as FinalBossTypeEnemyScriptableObject;
            stateMachine = stateMachineToSet;

            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        public override void Update()
        {
            if (finalBossController.IsDead || finalBossView.IsDying)
            {
                if (finalBossController.IsDead && !finalBossView.IsDying)
                    stateMachine.ChangeState(EnemyStates.DEATH);
                return;
            }

            if (finalBossController.IsPlayerDead || playerTransform == null)
            {
                if (finalBossController.IsPlayerDead)
                    finalBossView.StopMovement();

                stateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            float distanceToPlayer = Vector2.Distance(finalBossView.transform.position, playerTransform.position);

            if (distanceToPlayer > bossData.DetectionRange)
            {
                stateMachine.ChangeState(EnemyStates.IDLE);
                return;
            }

            if (distanceToPlayer > bossData.AttackRange && distanceToPlayer <= bossData.DetectionRange)
            {
                stateMachine.ChangeState(EnemyStates.RUNNING);
                return;
            }

            finalBossView.FaceTarget(playerTransform.position);
            TryAttack();
        }

        private void TryAttack()
        {
            if (finalBossController.IsPlayerDead || finalBossController.IsDead || finalBossView.IsDying)
                return;

            if (Time.time < finalBossModel.lastAttackTime + finalBossModel.AttackCooldown)
                return;

            if (!finalBossView.IsAttacking)
            {
                finalBossView.StartAttack();
                finalBossModel.lastAttackTime = Time.time;
            }
        }
    }
}