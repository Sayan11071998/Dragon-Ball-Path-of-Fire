using UnityEngine;

namespace DragonBall.Enemy
{
    public class BuuEnemyController : BaseEnemyController
    {
        public BuuEnemyController(EnemyScriptableObject enemySO, BaseEnemyView view, EnemyPool pool)
            : base(enemySO, view, pool)
        {
        }

        protected override void InitializeModel(EnemyScriptableObject enemySO)
        {
            enemyModel = new BuuEnemyModel(enemySO.MaxHealth, enemySO.AttackDamage, enemySO.AttackCooldown);
        }

        // Override base methods if needed or add Buu-specific methods
    }
}