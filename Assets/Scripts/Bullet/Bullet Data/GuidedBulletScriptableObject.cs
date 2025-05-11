using UnityEngine;

namespace DragonBall.Bullet.BulletData
{
    [CreateAssetMenu(fileName = "GuidedBulletScriptableObject", menuName = "Bullet/GuidedBulletScriptableObject")]
    public class GuidedBulletScriptableObject : BulletScriptableObject
    {
        [Header("Guided Bullet Properties")]
        [Range(60f, 360f)]
        public float RotationSpeed = 120f;

        [Range(0f, 5f)]
        public float GuidanceDelay = 0.5f;

        [Range(0f, 10f)]
        public float MaxGuidanceTime = 5f;
    }
}