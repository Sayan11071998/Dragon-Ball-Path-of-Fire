using DragonBall.Core;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private bool spawnOnStart = true;

        private void Start()
        {
            if (spawnOnStart)
            {
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()
        {
            GameService.Instance.enemyService.SpawnEnemy(enemyType, transform.position);
        }
    }
}