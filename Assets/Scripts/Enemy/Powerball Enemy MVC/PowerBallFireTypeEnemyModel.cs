using DragonBall.Enemy.ParentMVC;

namespace DragonBall.Enemy.PowerBallEnemyMVC
{
    public class PowerBallFireTypeEnemyModel : BaseEnemyModel
    {
        public PowerBallFireTypeEnemyModel(float _maxHealth, float _attackDamage, float _attackCooldown)
            : base(_maxHealth, _attackDamage, _attackCooldown) { }
    }
}