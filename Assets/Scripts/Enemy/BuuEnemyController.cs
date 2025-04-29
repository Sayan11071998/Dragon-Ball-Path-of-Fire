namespace DragonBall.Enemy
{
    public class BuuEnemyController : BaseEnemyController
    {
        public BuuEnemyController(EnemyScriptableObject enemySO, BaseEnemyView view, EnemyPool pool) : base(enemySO, view, pool) { }

        protected override void InitializeModel(EnemyScriptableObject enemySO)
        {
            baseEnemyModel = new BuuEnemyModel(enemySO.MaxHealth, enemySO.AttackDamage, enemySO.AttackCooldown);
        }
    }
}