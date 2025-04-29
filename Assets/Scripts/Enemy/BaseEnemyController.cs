using DragonBall.Core;

namespace DragonBall.Enemy
{
    public abstract class BaseEnemyController
    {
        protected BaseEnemyModel baseEnemyModel;
        protected BaseEnemyView baseEnemyView;
        protected EnemyPool enemyPool;
        protected EnemyScriptableObject enemyScriptableObject;
        protected EnemyStateMachine enemyStateMachine;

        public BaseEnemyView BaseEnemyView => baseEnemyView;
        public BaseEnemyModel BaseEnemyModel => baseEnemyModel;
        public EnemyScriptableObject EnemyData => enemyScriptableObject;

        protected bool isDead = false;
        public bool IsDead => isDead;

        protected bool isPlayerDead = false;
        public bool IsPlayerDead => isPlayerDead;

        public BaseEnemyController(EnemyScriptableObject enemyScriptableObjectToSet, BaseEnemyView viewToSet, EnemyPool poolToSet)
        {
            enemyScriptableObject = enemyScriptableObjectToSet;
            baseEnemyView = viewToSet;
            enemyPool = poolToSet;

            viewToSet.SetController(this);

            InitializeModel(enemyScriptableObjectToSet);

            enemyStateMachine = new EnemyStateMachine(this);

            CheckPlayerStatus();
            enemyStateMachine.ChangeState(EnemyStates.IDLE);
        }

        protected abstract void InitializeModel(EnemyScriptableObject _enemyData);

        public virtual void Update()
        {
            if (isDead) return;

            CheckPlayerStatus();
            enemyStateMachine.Update();
        }

        protected virtual void CheckPlayerStatus()
        {
            bool wasPlayerDead = isPlayerDead;
            isPlayerDead = GameService.Instance.playerService.PlayerController.PlayerModel.CurrentHealth <= 0;

            if (!wasPlayerDead && isPlayerDead)
                HandlePlayerDeath();
        }

        protected virtual void HandlePlayerDeath()
        {
            baseEnemyView.StopMovement();
            baseEnemyView.ResetAllInputs();
            enemyStateMachine.ChangeState(EnemyStates.IDLE);
        }

        public virtual void TakeDamage(float damage)
        {
            baseEnemyModel.TakeDamage(damage);

            if (baseEnemyModel.CurrentHealth <= 0 && !isDead)
                HandleDeath();
        }

        protected virtual void HandleDeath()
        {
            isDead = true;
            enemyStateMachine.ChangeState(EnemyStates.DEATH);
        }

        public virtual void OnDeathAnimationComplete() => Die();

        protected virtual void Die()
        {
            baseEnemyView.gameObject.SetActive(false);
            enemyPool.ReturnItem(this);
        }

        public virtual void Reset()
        {
            baseEnemyModel.Reset();
            isDead = false;
            isPlayerDead = false;
            enemyStateMachine.ChangeState(EnemyStates.IDLE);
        }
    }
}