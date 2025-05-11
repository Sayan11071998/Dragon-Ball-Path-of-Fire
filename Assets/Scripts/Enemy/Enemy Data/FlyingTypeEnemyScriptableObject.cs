using UnityEngine;

namespace DragonBall.Enemy.EnemyData
{
    [CreateAssetMenu(fileName = "FlyingTypeEnemyScriptableObject", menuName = "Enemy/FlyingTypeEnemyScriptableObject")]
    public class FlyingTypeEnemyScriptableObject : EnemyScriptableObject
    {
        [Header("Flying-Specific Settings")]
        public float FloatAmplitude = 0.5f;
        public float FloatSpeed = 1.0f;
        public float AerialMoveSpeed = 4.0f;
        public float GuidedBulletDamage = 15f;
    }
}