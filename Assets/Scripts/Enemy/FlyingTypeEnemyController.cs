namespace DragonBall.Enemy
{
    public class FlyingTypeEnemyController : BaseEnemyController
    {
        public FlyingTypeEnemyController(EnemyScriptableObject enemySO, BaseEnemyView view, EnemyPool pool)
            : base(enemySO, view, pool) { }

        protected override void InitializeModel(EnemyScriptableObject enemySO)
        {
            baseEnemyModel = new FlyingTypeEnemyModel(enemySO.MaxHealth, enemySO.AttackDamage, enemySO.AttackCooldown);
        }
    }
}