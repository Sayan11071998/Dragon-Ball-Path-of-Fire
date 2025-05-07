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

        protected override void InitializeModel(EnemyScriptableObject enemySO)
        {
            var bossSO = enemySO as FinalBossTypeEnemyScriptableObject;
            float maxHealth = bossSO != null ? bossSO.MaxHealth : enemySO.MaxHealth;
            float attackDamage = bossSO != null ? bossSO.AttackDamage : enemySO.AttackDamage;
            float attackCooldown = bossSO != null ? bossSO.AttackCooldown : enemySO.AttackCooldown;

            baseEnemyModel = new FinalBossTypeEnemyModel(maxHealth, attackDamage, attackCooldown);
            bossModel = baseEnemyModel as FinalBossTypeEnemyModel;
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage * 0.75f);

            if (baseEnemyModel.CurrentHealth < baseEnemyModel.MaxHealth * 0.5f)
            {
                bossModel.EnterEnragedState();
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Reset()
        {
            base.Reset();
            bossModel.ResetEnragedState();
            lastSpecialAttackTime = -Mathf.Infinity;
        }
    }
}