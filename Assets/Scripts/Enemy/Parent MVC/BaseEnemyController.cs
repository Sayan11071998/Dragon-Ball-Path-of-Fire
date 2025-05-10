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

            InitializeModel();
            UpdateHealthBar();

            InitializeStateMachine();

            CheckPlayerStatus();
            enemyStateMachine.ChangeState(EnemyStates.IDLE);
        }

        protected virtual void InitializeModel() => baseEnemyModel = CreateModel(enemyScriptableObject);

        protected virtual BaseEnemyModel CreateModel(EnemyScriptableObject enemyData)
        {
            return new BaseEnemyModel(
                enemyData.MaxHealth,
                enemyData.AttackDamage,
                enemyData.AttackCooldown
            );
        }

        protected virtual void InitializeStateMachine()
        {
            enemyStateMachine = new EnemyStateMachine(this);
        }

        public virtual void Update()
        {
            if (isDead) return;

            CheckPlayerStatus();
            enemyStateMachine.Update();
        }

        protected virtual void CheckPlayerStatus()
        {
            bool wasPlayerDead = isPlayerDead;
            isPlayerDead = IsPlayerHealthDepleted();

            if (!wasPlayerDead && isPlayerDead)
                HandlePlayerDeath();
        }

        protected virtual bool IsPlayerHealthDepleted() => GameService.Instance.playerService.PlayerController.PlayerModel.CurrentHealth <= 0;

        protected virtual void HandlePlayerDeath()
        {
            baseEnemyView.StopMovement();
            baseEnemyView.ResetAllInputs();
            enemyStateMachine.ChangeState(EnemyStates.IDLE);
        }

        public virtual void TakeDamage(float damage)
        {
            baseEnemyModel.TakeDamage(damage);
            UpdateHealthBar();

            if (baseEnemyModel.CurrentHealth <= 0 && !isDead)
                HandleDeath();
        }

        protected virtual void UpdateHealthBar() => baseEnemyView.UpdateHealthBar(baseEnemyModel.MaxHealth, baseEnemyModel.CurrentHealth);

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

            UpdateHealthBar();
            enemyStateMachine.ChangeState(EnemyStates.IDLE);
        }
    }
}