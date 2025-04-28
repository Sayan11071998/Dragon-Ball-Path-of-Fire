using UnityEngine;
using DragonBall.Core;

public class EnemySpawner : MonoBehaviour
{
    private void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        GameService.Instance.enemyService.SpawnEnemy(transform.position);
    }
}