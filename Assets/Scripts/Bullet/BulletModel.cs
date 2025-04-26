namespace DragonBall.Bullet
{
    public class BulletModel
    {
        public float Speed { get; private set; }
        public int Damage { get; private set; }
        public float Lifetime { get; private set; }

        public BulletModel(BulletScriptableObject config)
        {
            Speed = config.BulletSpeed;
            Damage = config.BulletDamage;
            Lifetime = config.BulletLifetime;
        }
    }
}