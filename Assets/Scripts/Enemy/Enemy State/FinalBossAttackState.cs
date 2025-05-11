using DragonBall.Enemy.EnemyUtilities;
using DragonBall.Enemy.FinalBossEnemyMVC;
using DragonBall.Enemy.ParentMVC;
using UnityEngine;

namespace DragonBall.Enemy.EnemyState
{
    public class FinalBossAttackState : AttackState
    {
        private FinalBossTypeEnemyModel finalBossModel;

        public FinalBossAttackState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
            : base(controllerToSet, stateMachineToSet)
        {
            finalBossModel = controllerToSet.BaseEnemyModel as FinalBossTypeEnemyModel;
        }

        protected override bool IsAttackCooldownComplete()
        {
            if (finalBossModel != null && finalBossModel.IsEnraged)
                return Time.time >= finalBossModel.lastAttackTime + finalBossModel.AttackCooldown;

            return base.IsAttackCooldownComplete();
        }

        protected override bool CanAttack() => base.CanAttack() && !finalBossModel.IsRegenerating;
    }
}