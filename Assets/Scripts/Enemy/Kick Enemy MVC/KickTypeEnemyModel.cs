using DragonBall.Enemy.ParentMVC;

namespace DragonBall.Enemy.KickEnemyMVC
{
    public class KickTypeEnemyModel : BaseEnemyModel
    {
        public KickTypeEnemyModel(float _maxHealth, float _attackDamage, float _attackCooldown)
            : base(_maxHealth, _attackDamage, _attackCooldown) { }
    }
}