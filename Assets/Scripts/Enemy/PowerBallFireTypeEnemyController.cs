namespace DragonBall.Enemy
{
    public class PowerBallFireTypeEnemyController : BaseEnemyController
    {
        public PowerBallFireTypeEnemyController(EnemyScriptableObject enemySO, BaseEnemyView view, EnemyPool pool) : base(enemySO, view, pool) { }

        protected override void InitializeModel(EnemyScriptableObject enemySO)
        {
            baseEnemyModel = new PowerBallFireTypeEnemyModel(enemySO.MaxHealth, enemySO.AttackDamage, enemySO.AttackCooldown);
        }
    }
}