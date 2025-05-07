using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class FinalBossAttackState : AttackState
    {
        private FinalBossTypeEnemyController finalBossController;
        private FinalBossTypeEnemyModel finalBossModel;
        private FinalBossTypeEnemyView finalBossView;
        private FinalBossTypeEnemyScriptableObject bossData;

        public FinalBossAttackState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
            : base(controllerToSet, stateMachineToSet)
        {
            finalBossController = controllerToSet as FinalBossTypeEnemyController;
            finalBossModel = finalBossController.BaseEnemyModel as FinalBossTypeEnemyModel;
            finalBossView = finalBossController.BaseEnemyView as FinalBossTypeEnemyView;
            bossData = controllerToSet.EnemyData as FinalBossTypeEnemyScriptableObject;
        }

        protected override void TryAttack()
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