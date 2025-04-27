using UnityEngine;

namespace DragonBall.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Enemy")]
    public class EnemyScriptableObject : ScriptableObject
    {
        public EnemyType EnemyType;
        public EnemyView EnemyPrefab;
        public float MaxHealth = 100f;
        public float MovementSpeed = 3f;
        public float DetectionRange = 10f;
        public float AttackRange = 2f;
    }
}