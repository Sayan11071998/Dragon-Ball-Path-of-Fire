using UnityEngine;
using DragonBall.Core;

namespace DragonBall.Enemy
{
    public class EnemyController : IDamageable
    {
        private EnemyModel enemyModel;
        private EnemyView enemyView;
        private EnemyPool enemyPool;

        private Transform playerTransform;
        private bool hasDetectedPlayer = false;
        
        public bool NeedsInitialization => enemyModel == null;
        public EnemyType EnemyType => enemyModel?.EnemyType ?? EnemyType.BUU;

        public EnemyController(EnemyView _view)
        {
            enemyView = _view;
            enemyView.SetController(this);
        }
        
        public void SetModel(EnemyModel model)
        {
            enemyModel = model;
        }
        
        public void SetPool(EnemyPool pool)
        {
            enemyPool = pool;
        }

        public void Initialize(Vector3 spawnPosition)
        {
            // Reset state
            hasDetectedPlayer = false;
            
            // Reset health if needed
            if (enemyModel != null && enemyModel.IsDead)
            {
                enemyModel.ResetHealth();
            }
            
            // Set position
            enemyView.SetPosition(spawnPosition);
            
            // Find player
            playerTransform = GameService.Instance.playerService.PlayerPrefab.transform;
            
            // Activate the view
            enemyView.gameObject.SetActive(true);
        }

        public void Update()
        {
            if (enemyModel == null || enemyModel.IsDead) return;

            float distance = Vector3.Distance(enemyView.transform.position, playerTransform.position);

            if (!hasDetectedPlayer && distance <= enemyModel.DetectionRange)
                hasDetectedPlayer = true;

            if (hasDetectedPlayer)
            {
                if (distance > enemyModel.AttackRange)
                {
                    enemyView.MoveTo(playerTransform.position, enemyModel.MovementSpeed);
                }
                else
                {
                    enemyView.Attack();
                }
            }
        }

        public void Damage(float damageAmount)
        {
            if (enemyModel == null || enemyModel.IsDead) return;
            
            enemyModel.TakeDamage(damageAmount);
            
            if (enemyModel.CurrentHealth <= 0 && !enemyModel.IsDead)
            {
                enemyModel.IsDead = true;
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            enemyView.Die();
            GameService.Instance.enemyService.HandleEnemyDeath(this);
        }
        
        public void ReturnToPool()
        {
            enemyView.gameObject.SetActive(false);
        }
    }
}