using DragonBall.Core;

namespace DragonBall.Enemy
{
    public class EnemyController
    {
        private EnemyModel model;
        private EnemyView view;
        private EnemyPool pool;
        private EnemyScriptableObject enemySO;

        public EnemyView View => view;

        public EnemyController(EnemyScriptableObject enemySO, EnemyView view)
        {
            this.enemySO = enemySO;
            this.view = view;
            model = new EnemyModel(enemySO);
            view.SetController(this);
        }

        public void TakeDamage(float damage)
        {
            model.TakeDamage(damage);
            if (model.CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            GameService.Instance.enemyService.ReturnEnemy(view);
        }
    }
}