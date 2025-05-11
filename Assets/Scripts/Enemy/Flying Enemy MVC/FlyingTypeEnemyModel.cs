using DragonBall.Enemy.ParentMVC;

namespace DragonBall.Enemy.FlyingEnemyMVC
{
    public class FlyingTypeEnemyModel : BaseEnemyModel
    {
        public FlyingTypeEnemyModel(float _maxHealth, float _attackDamage, float _attackCooldown)
            : base(_maxHealth, _attackDamage, _attackCooldown) { }
    }
}