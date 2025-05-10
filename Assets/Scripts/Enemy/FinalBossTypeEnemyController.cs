using DragonBall.Core;
using DragonBall.Sound;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class FinalBossTypeEnemyController : BaseEnemyController
    {
        private FinalBossTypeEnemyModel bossModel;
        private FinalBossTypeEnemyView bossView;

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
                model.SpecialAttackCooldown = bossSO.SpecialAttackCooldown;

            return model;
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage * 0.75f);
            FinalBossTypeEnemyModel finalBossModel = baseEnemyModel as FinalBossTypeEnemyModel;

            if (finalBossModel != null && baseEnemyModel.CurrentHealth < baseEnemyModel.MaxHealth * 0.5f && !finalBossModel.HasRegeneratedHealth && !isDead)
            {
                finalBossModel.RegenerateHealth();
                SoundManager.Instance.PlaySoundEffect(SoundType.FinalBossTransformation);
                bossView.StartRegenerationAnimation();
            }
            else if (baseEnemyModel.CurrentHealth < baseEnemyModel.MaxHealth * 0.5f)
            {
                finalBossModel?.EnterEnragedState();
            }
        }

        public void OnRegenerationAnimationComplete()
        {
            FinalBossTypeEnemyModel finalBossModel = baseEnemyModel as FinalBossTypeEnemyModel;
            finalBossModel?.CompleteRegeneration();
            UpdateHealthBar();
        }

        public override void Reset()
        {
            base.Reset();
            bossModel.ResetEnragedState();
        }

        protected override void HandleDeath()
        {
            base.HandleDeath();
            GameService.Instance.playerService.PlayerController.DisablePlayerController();
            GameService.Instance.uiService.ShowGameCompletePanel();
        }
    }
}