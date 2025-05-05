using UnityEngine;

namespace DragonBall.Enemy
{
    [CreateAssetMenu(fileName = "FlyingTypeEnemy", menuName = "Enemy/FlyingTypeEnemy")]
    public class FlyingTypeEnemyScriptableObject : EnemyScriptableObject
    {
        [Header("Flying-Specific Settings")]
        public float FloatAmplitude = 0.5f;
        public float FloatSpeed = 1.0f;
        public float AerialMoveSpeed = 4.0f; // Can be faster than ground enemies
        public float GuidedBulletDamage = 15f; // Special damage for guided bullets
    }
}