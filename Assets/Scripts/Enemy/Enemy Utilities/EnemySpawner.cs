using UnityEngine;
using DragonBall.Core;

namespace DragonBall.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] public EnemyType enemyType;

        private void Start() => SpawnEnemy();

        private void SpawnEnemy()
        {
            BaseEnemyController enemy = GameService.Instance.enemyService.SpawnEnemy(enemyType);

            if (enemy != null)
                enemy.BaseEnemyView.transform.position = transform.position;
            else
                Debug.LogError($"Failed to spawn enemy of type {enemyType}");
        }
    }
}