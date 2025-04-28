using UnityEngine;
using DragonBall.Enemy;
using DragonBall.Core;

namespace DragonBall.Environment
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;

        private void Start() => SpawnEnemy();

        public void SpawnEnemy() => GameService.Instance.enemyService.SpawnEnemy(enemyType, transform.position);
    }
}