using UnityEngine;

namespace DragonBall.Enemy.EnemyData
{
    [CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Enemy/EnemyScriptableObject")]
    public class EnemyScriptableObject : ScriptableObject
    {
        [Header("Stats")]
        public float MaxHealth = 100f;

        [Header("Movement")]
        public float MoveSpeed = 3f;
        public float DetectionRange = 10f;
        public float AttackRange = 2f;

        [Header("Attack")]
        public float AttackDamage = 10f;
        public float AttackCooldown = 1f;

        [Header("State Transition Settings")]
        public float ExitAttackBuffer = 0.3f;
    }
}