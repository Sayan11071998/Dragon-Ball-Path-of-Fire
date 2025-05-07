using UnityEngine;

namespace DragonBall.Enemy
{
    public class FinalBossTypeEnemyController : BaseEnemyController
    {
        private FinalBossTypeEnemyModel bossModel;
        private FinalBossTypeEnemyView bossView;
        private float lastSpecialAttackTime = -Mathf.Infinity;

        public FinalBossTypeEnemyController(EnemyScriptableObject enemySO, BaseEnemyView view, EnemyPool pool)
            : base(enemySO, view, pool)
        {
            bossModel = baseEnemyModel as FinalBossTypeEnemyModel;
            bossView = baseEnemyView as FinalBossTypeEnemyView;
        }

        protected override BaseEnemyModel CreateModel(EnemyScriptableObject enemyData)
        {
            var bossSO = enemyData as FinalBossTypeEnemyScriptableObject;
            float maxHealth = bossSO != null ? bossSO.MaxHealth : enemyData.MaxHealth;
            float attackDamage = bossSO != null ? bossSO.AttackDamage : enemyData.AttackDamage;
            float attackCooldown = bossSO != null ? bossSO.AttackCooldown : enemyData.AttackCooldown;

            var model = new FinalBossTypeEnemyModel(maxHealth, attackDamage, attackCooldown);

            if (bossSO != null)
            {
                model.SpecialAttackCooldown = bossSO.SpecialAttackCooldown;
            }

            return model;
        }

        public override void TakeDamage(float damage)
        {
            // Apply damage reduction for boss type
            base.TakeDamage(damage * 0.75f);

            // Check for enraged state
            if (baseEnemyModel.CurrentHealth < baseEnemyModel.MaxHealth * 0.5f)
            {
                bossModel.EnterEnragedState();
            }
        }

        public override void Reset()
        {
            base.Reset();

            bossModel.ResetEnragedState();
            lastSpecialAttackTime = -Mathf.Infinity;
        }
    }
}