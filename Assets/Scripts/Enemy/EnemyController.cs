namespace DragonBall.Enemy
{
    public class EnemyController
    {
        private EnemyModel enemyModel;
        private EnemyView enemyView;
        private EnemyPool enemyPool;
        private EnemyScriptableObject enemyScriptableObject;
        private EnemyStateMachine enemyStateMachine;

        public EnemyView EnemyView => enemyView;
        public EnemyModel EnemyModel => enemyModel;
        public EnemyScriptableObject EnemyData => enemyScriptableObject;

        private bool isDead = false;
        public bool IsDead => isDead;

        public EnemyController(EnemyScriptableObject enemySO, EnemyView view, EnemyPool pool)
        {
            enemyScriptableObject = enemySO;
            enemyView = view;
            enemyPool = pool;

            enemyModel = new EnemyModel(enemySO.MaxHealth, enemySO.AttackDamage, enemySO.AttackCooldown);
            view.SetController(this);

            enemyStateMachine = new EnemyStateMachine(this);
            enemyStateMachine.ChangeState(EnemyStates.IDLE);
        }

        public void Update()
        {
            if (isDead) return;
            enemyStateMachine.Update();
        }

        public void TakeDamage(float damage)
        {
            enemyModel.TakeDamage(damage);

            if (enemyModel.CurrentHealth <= 0 && !isDead)
                HandleDeath();
        }

        private void HandleDeath()
        {
            isDead = true;
            enemyStateMachine.ChangeState(EnemyStates.DEATH);
        }

        public void OnDeathAnimationComplete() => Die();

        private void Die()
        {
            enemyView.gameObject.SetActive(false);
            enemyPool.ReturnItem(this);
        }

        public void Reset()
        {
            enemyModel.Reset();
            isDead = false;
            enemyStateMachine.ChangeState(EnemyStates.IDLE);
        }
    }
}