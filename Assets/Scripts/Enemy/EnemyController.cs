using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyController
    {
        private EnemyModel model;
        private EnemyView view;
        private EnemyPool pool;
        private EnemyScriptableObject enemySO;

        private Transform playerTransform;
        private bool isPlayerDetected = false;
        private bool isInAttackRange = false;

        public EnemyView View => view;

        public EnemyController(EnemyScriptableObject enemySO, EnemyView view, EnemyPool pool)
        {
            this.enemySO = enemySO;
            this.view = view;
            this.pool = pool;
            model = new EnemyModel(enemySO.MaxHealth);
            view.SetController(this);
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        public void FixedUpdate()
        {
            if (playerTransform == null)
                return;

            float distanceToPlayer = Vector2.Distance(view.transform.position, playerTransform.position);

            isPlayerDetected = distanceToPlayer <= enemySO.DetectionRange;
            isInAttackRange = distanceToPlayer <= enemySO.AttackRange;

            bool shouldMove = isPlayerDetected && !isInAttackRange;
            view.SetMoving(shouldMove);

            if (shouldMove)
            {
                Vector2 direction = ((Vector2)playerTransform.position - (Vector2)view.transform.position).normalized;
                view.MoveInDirection(direction, enemySO.MoveSpeed);
            }
            else if (isInAttackRange)
            {
                view.FaceTarget(playerTransform.position);
            }
        }

        public void TakeDamage(float damage)
        {
            model.TakeDamage(damage);

            if (model.CurrentHealth <= 0)
                Die();
        }

        private void Die()
        {
            view.gameObject.SetActive(false);
            pool.ReturnItem(this);
        }

        public void Reset()
        {
            model.Reset();
            isPlayerDetected = false;
            isInAttackRange = false;
        }
    }
}