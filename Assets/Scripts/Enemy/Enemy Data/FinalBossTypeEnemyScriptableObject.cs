using UnityEngine;

namespace DragonBall.Enemy.EnemyData
{
    [CreateAssetMenu(fileName = "FinalBossTypeEnemyScriptableObject", menuName = "Enemy/FinalBossTypeEnemyScriptableObject")]
    public class FinalBossTypeEnemyScriptableObject : EnemyScriptableObject
    {
        [Header("Boss-Specific Settings")]
        public float FloatAmplitude = 0.7f;
        public float FloatSpeed = 0.8f;
        public float AerialMoveSpeed = 3.5f;
        public float GuidedBulletDamage = 25f;

        [Header("Special Attack Settings")]
        public float SpecialAttackCooldown = 15f;
        public float SpecialAttackDamage = 40f;
        public float SpecialAttackRange = 12f;
    }
}