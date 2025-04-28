using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyController
    {
        private EnemyView enemyView;
        private EnemyModel enemyModel;
        public EnemyType EnemyType { get; private set; }

        public EnemyController(EnemyView view, EnemyScriptableObject enemyData)
        {
            enemyView = view;
            enemyView.SetController(this);
            EnemyType = enemyView.GetEnemyType();

            enemyModel = new EnemyModel(
                enemyData.EnemyType,
                enemyData.MaxHealth,
                enemyData.MovementSpeed,
                enemyData.DetectionRange
            );
        }

        public void Initialize(Vector3 spawnPosition)
        {
            enemyView.SetPosition(spawnPosition);
            enemyModel.ResetHealth();
            enemyView.gameObject.SetActive(true);
        }

        public void ReturnToPool() => enemyView.gameObject.SetActive(false);
    }
}