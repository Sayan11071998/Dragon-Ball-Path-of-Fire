namespace DragonBall.Bullet
{
    public class GuidedBulletModel : BulletModel
    {
        public float RotationSpeed { get; private set; }
        public float GuidanceDelay { get; private set; }
        public float MaxGuidanceTime { get; private set; }

        public GuidedBulletModel(float speed, float damage, float lifetime, float rotationSpeed, float guidanceDelay, float maxGuidanceTime)
            : base(speed, damage, lifetime)
        {
            RotationSpeed = rotationSpeed;
            GuidanceDelay = guidanceDelay;
            MaxGuidanceTime = maxGuidanceTime;
        }
    }
}