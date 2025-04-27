using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyService
    {
        private EnemyModel enemyModel;
        private EnemyController enemyController;
        private EnemyView enemyPrefab;

        public EnemyView EnemyPrefab => enemyPrefab;
        public EnemyController EnemyController => enemyController;

        public EnemyService(EnemyView _enemyPrefab, EnemyScriptableObject _config)
        {
            enemyPrefab = Object.Instantiate(_enemyPrefab);

            enemyModel = new EnemyModel
                (
                    _config.EnemyType,
                    _config.MaxHealth,
                    _config.MovementSpeed,
                    _config.DetectionRange,
                    _config.AttackRange
                );

            enemyController = new EnemyController(enemyModel, enemyPrefab);
        }

        public void Update() => enemyController.Update();
    }
}