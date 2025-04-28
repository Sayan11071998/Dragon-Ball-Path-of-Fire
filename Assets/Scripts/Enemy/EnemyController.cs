namespace DragonBall.Enemy
{
    public class EnemyController
    {
        private EnemyModel model;
        private EnemyView view;
        private EnemyPool pool;
        private EnemyScriptableObject enemySO;

        public EnemyView View => view;

        public EnemyController(EnemyScriptableObject enemySO, EnemyView view, EnemyPool pool)
        {
            this.enemySO = enemySO;
            this.view = view;
            this.pool = pool;
            model = new EnemyModel(enemySO.MaxHealth);
            view.SetController(this);
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

        public void Reset() => model.Reset();
    }
}