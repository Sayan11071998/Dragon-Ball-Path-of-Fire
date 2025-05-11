using UnityEngine;
using DragonBall.Core;
using DragonBall.Enemy.EnemyData;
using DragonBall.Enemy.ParentMVC;

namespace DragonBall.Enemy.EnemyUtilities
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
        }
    }
}