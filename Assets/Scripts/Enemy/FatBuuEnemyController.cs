namespace DragonBall.Enemy
{
    public class FatBuuEnemyController : BaseEnemyController
    {
        public FatBuuEnemyController(EnemyScriptableObject enemySO, BaseEnemyView view, EnemyPool pool) : base(enemySO, view, pool) { }

        protected override void InitializeModel(EnemyScriptableObject enemySO)
        {
            baseEnemyModel = new FatBuuEnemyModel(enemySO.MaxHealth, enemySO.AttackDamage, enemySO.AttackCooldown);
        }
    }
}