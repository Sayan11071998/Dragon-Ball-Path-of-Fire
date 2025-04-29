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
            EnemyController enemy = GameService.Instance.enemyService.SpawnEnemy(enemyType);
            if (enemy != null)
                enemy.EnemyView.transform.position = transform.position;
        }
    }
}