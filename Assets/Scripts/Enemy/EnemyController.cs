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
            model = new EnemyModel(enemySO.MaxHealth, enemySO.AttackDamage, enemySO.AttackCooldown);
            view.SetController(this);
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        public void FixedUpdate()
        {
            if (playerTransform == null)
                return;

            UpdateDetection();

            if (ShouldMove())
                HandleMovement();
            else if (isInAttackRange)
                HandleAttack();
        }

        private void UpdateDetection()
        {
            float distanceToPlayer = Vector2.Distance(view.transform.position, playerTransform.position);

            isPlayerDetected = distanceToPlayer <= enemySO.DetectionRange;
            isInAttackRange = distanceToPlayer <= enemySO.AttackRange;
        }

        private bool ShouldMove() => isPlayerDetected && !isInAttackRange;

        private void HandleMovement()
        {
            view.SetMoving(true);
            Vector2 direction = ((Vector2)playerTransform.position - (Vector2)view.transform.position).normalized;
            view.MoveInDirection(direction, enemySO.MoveSpeed);
        }

        private void HandleAttack()
        {
            view.SetMoving(false);
            view.FaceTarget(playerTransform.position);
            TryAttack();
        }

        private void TryAttack()
        {
            if (Time.time < model.lastAttackTime + enemySO.AttackCooldown)
                return;

            if (!view.IsAttacking)
            {
                view.StartAttack();
                model.lastAttackTime = Time.time;
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