namespace DragonBall.Bullet.ParentMVC
{
    public class BulletModel
    {
        public float Speed { get; private set; }
        public float Damage { get; private set; }
        public float Lifetime { get; private set; }

        public BulletModel(float _speed, float _damage, float _lifetime)
        {
            Speed = _speed;
            Damage = _damage;
            Lifetime = _lifetime;
        }
    }
}