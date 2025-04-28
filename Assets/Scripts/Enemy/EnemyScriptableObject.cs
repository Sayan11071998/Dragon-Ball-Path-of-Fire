using UnityEngine;

namespace DragonBall.Enemy
{
    [CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Enemy/EnemyScriptableObject")]
    public class EnemyScriptableObject : ScriptableObject
    {
        [Header("Prefab")]
        public EnemyView EnemyPrefab;

        [Header("Stats")]
        public float MaxHealth = 100f;
    }
}