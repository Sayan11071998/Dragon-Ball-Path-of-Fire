using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyController
    {
        private EnemyModel model;
        private EnemyView view;
        private EnemyPool pool;
        private EnemyScriptableObject enemySO;
        private EnemyStateMachine stateMachine;

        private bool isDead = false;

        public EnemyView View => view;
        public EnemyModel Model => model;
        public EnemyScriptableObject EnemySO => enemySO;
        public bool IsDead => isDead;

        public EnemyController(EnemyScriptableObject enemySO, EnemyView view, EnemyPool pool)
        {
            this.enemySO = enemySO;
            this.view = view;
            this.pool = pool;
            model = new EnemyModel(enemySO.MaxHealth, enemySO.AttackDamage, enemySO.AttackCooldown);
            view.SetController(this);

            // Initialize and setup state machine
            stateMachine = new EnemyStateMachine(this);
            stateMachine.ChangeState(EnemyStates.IDLE);
        }

        public void FixedUpdate()
        {
            if (isDead)
                return;

            stateMachine.Update();
        }

        public void TakeDamage(float damage)
        {
            model.TakeDamage(damage);

            if (model.CurrentHealth <= 0 && !isDead)
                HandleDeath();
        }

        private void HandleDeath()
        {
            isDead = true;
            stateMachine.ChangeState(EnemyStates.DEATH);
        }

        public void OnDeathAnimationComplete()
        {
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
            isDead = false;
            stateMachine.ChangeState(EnemyStates.IDLE);
        }
    }
}