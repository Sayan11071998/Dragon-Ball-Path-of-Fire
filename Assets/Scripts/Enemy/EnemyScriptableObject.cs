using UnityEngine;

namespace DragonBall.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Enemy")]
    public class EnemyScriptableObject : ScriptableObject
    {
        public EnemyType enemyType;
        public EnemyView enemyPrefab;
        public float maxHealth = 100f;
        public float movementSpeed = 3f;
        public float detectionRange = 10f;
        public float attackRange = 2f;
    }
}