using UnityEngine;
using DragonBall.Core;

namespace DragonBall.Enemy
{
    public class EnemyController
    {
        private readonly EnemyModel model;
        private readonly EnemyView view;
        private readonly Transform playerTransform;
        private bool hasDetectedPlayer = false;

        public EnemyView View => view;
        public EnemyType Type => model.Type;

        public EnemyController(EnemyModel model, EnemyView view)
        {
            this.model = model;
            this.view = view;
            view.Initialize(this);
            playerTransform = GameService.Instance.playerService.PlayerPrefab.transform;
        }

        public void Initialize(Vector3 spawnPosition)
        {
            view.SetPosition(spawnPosition);
        }

        public void Update()
        {
            if (model.IsDead) return;

            float distance = Vector3.Distance(view.Transform.position, playerTransform.position);

            if (!hasDetectedPlayer && distance <= model.Data.detectionRange)
                hasDetectedPlayer = true;

            if (hasDetectedPlayer)
            {
                if (distance > model.Data.attackRange)
                {
                    view.MoveTo(playerTransform.position, model.Data.movementSpeed);
                }
                else
                {
                    view.Attack();
                }
            }
        }

        public void ReceiveDamage(float amount)
        {
            model.TakeDamage(amount);
            if (model.IsDead)
                HandleDeath();
        }

        private void HandleDeath()
        {
            view.Die();
            EnemyService.Instance.HandleEnemyDeath(this);
        }
    }
}