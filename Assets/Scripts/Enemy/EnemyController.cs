using UnityEngine;
using DragonBall.Core;

namespace DragonBall.Enemy
{
    public class EnemyController
    {
        private EnemyModel enemyModel;
        private EnemyView enemyView;
        private EnemyPool enemyPool;

        // private readonly Transform playerTransform;
        // private bool hasDetectedPlayer = false;

        // public EnemyView View => enemyView;
        // public EnemyType Type => enemyModel.Type;

        public EnemyController(EnemyModel _model, EnemyView _view, EnemyPool _pool)
        {
            enemyModel = _model;
            enemyView = _view;
            enemyPool = _pool;

            enemyView.SetController(this);
        }

        public void Initialize(Vector3 spawnPosition)
        {
            enemyView.SetPosition(spawnPosition);
        }

        // public void Update()
        // {
        //     if (enemyModel.IsDead) return;

        //     float distance = Vector3.Distance(enemyView.Transform.position, playerTransform.position);

        //     if (!hasDetectedPlayer && distance <= enemyModel.Data.detectionRange)
        //         hasDetectedPlayer = true;

        //     if (hasDetectedPlayer)
        //     {
        //         if (distance > enemyModel.Data.attackRange)
        //         {
        //             enemyView.MoveTo(playerTransform.position, enemyModel.Data.movementSpeed);
        //         }
        //         else
        //         {
        //             enemyView.Attack();
        //         }
        //     }
        // }

        // public void ReceiveDamage(float amount)
        // {
        //     enemyModel.TakeDamage(amount);
        //     if (enemyModel.IsDead)
        //         HandleDeath();
        // }

        // private void HandleDeath()
        // {
        //     enemyView.Die();
        //     EnemyService.Instance.HandleEnemyDeath(this);
        // }
    }
}