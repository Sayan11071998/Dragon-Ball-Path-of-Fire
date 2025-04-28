using UnityEngine;

namespace DragonBall.Enemy
{
    [CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "EnemyScriptableObject", order = 0)]
    public class EnemyScriptableObject : ScriptableObject
    {
        [Header("Prefab")]
        public EnemyView EnemyPrefab;

        [Header("Stats")]
        public float MaxHealth;
    }
}