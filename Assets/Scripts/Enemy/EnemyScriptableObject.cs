using UnityEngine;

namespace DragonBall.Enemy
{
    [CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Enemy/EnemyScriptableObject")]
    public class EnemyScriptableObject : ScriptableObject
    {
        [Header("Stats")]
        public int Health = 100;

        [Header("Movement")]
        public float MoveSpeed = 3f;

        [Header("Attack")]
        public float AttackPower = 10f;
        public float AttackRange = 1.5f;
        public float AttackCooldown = 2f;

        [Header("AI")]
        public float SensingRange = 10f;
        public float StoppingDistance = 2f;

        [Header("Prefab")]
        public EnemyView EnemyPrefab;
    }
}