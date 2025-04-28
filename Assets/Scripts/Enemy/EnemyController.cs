using UnityEngine;
using DragonBall.Core;

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
                enemyData.DetectionRange,
                enemyData.AttackRange
            );
        }

        public void Initialize(Vector3 spawnPosition)
        {
            enemyView.SetPosition(spawnPosition);
            enemyModel.ResetHealth();
            enemyView.gameObject.SetActive(true);
        }

        public void Update()
        {
            if (enemyModel.IsDead) return;

            Vector2 enemyPos = enemyView.transform.position;
            Vector2 playerPos = GameService.Instance.playerService.PlayerPrefab.transform.position;

            float distance = Vector2.Distance(enemyPos, playerPos);

            if (distance <= enemyModel.DetectionRange)
            {
                float directionX = playerPos.x - enemyPos.x;
                enemyView.SetFacingDirection(directionX);

                if (distance > enemyModel.AttackRange)
                {
                    Vector2 direction = (playerPos - enemyPos).normalized;
                    enemyView.SetVelocity(direction * enemyModel.MovementSpeed);
                    enemyView.SetRunning(true);
                }
                else
                {
                    enemyView.SetVelocity(Vector2.zero);
                    enemyView.SetRunning(false);
                }
            }
            else
            {
                enemyView.SetVelocity(Vector2.zero);
                enemyView.SetRunning(false);
            }
        }

        public void ReturnToPool()
        {
            enemyModel.ResetHealth();
            enemyView.gameObject.SetActive(false);
        }
    }
}