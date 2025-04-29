using DragonBall.Core;
using UnityEngine;

namespace DragonBall.Enemy
{
    public abstract class BaseEnemyController
    {
        protected BaseEnemyModel enemyModel;
        protected BaseEnemyView enemyView;
        protected EnemyPool enemyPool;
        protected EnemyScriptableObject enemyScriptableObject;
        protected EnemyStateMachine enemyStateMachine;

        public BaseEnemyView EnemyView => enemyView;
        public BaseEnemyModel EnemyModel => enemyModel;
        public EnemyScriptableObject EnemyData => enemyScriptableObject;

        protected bool isDead = false;
        public bool IsDead => isDead;

        protected bool _isPlayerDead = false;
        public bool isPlayerDead => _isPlayerDead;

        public BaseEnemyController(EnemyScriptableObject enemySO, BaseEnemyView view, EnemyPool pool)
        {
            enemyScriptableObject = enemySO;
            enemyView = view;
            enemyPool = pool;

            view.SetController(this);

            InitializeModel(enemySO);

            enemyStateMachine = new EnemyStateMachine(this);

            CheckPlayerStatus();
            enemyStateMachine.ChangeState(EnemyStates.IDLE);
        }

        protected abstract void InitializeModel(EnemyScriptableObject enemySO);

        public virtual void Update()
        {
            if (isDead) return;

            CheckPlayerStatus();
            enemyStateMachine.Update();
        }

        protected virtual void CheckPlayerStatus()
        {
            bool wasPlayerDead = _isPlayerDead;
            _isPlayerDead = GameService.Instance.playerService.PlayerController.PlayerModel.CurrentHealth <= 0;

            if (!wasPlayerDead && _isPlayerDead)
                HandlePlayerDeath();
        }

        protected virtual void HandlePlayerDeath()
        {
            enemyView.StopMovement();
            enemyView.ResetAllInputs();
            enemyStateMachine.ChangeState(EnemyStates.IDLE);
            Debug.Log("Enemy detected player death - stopping all actions");
        }

        public virtual void TakeDamage(float damage)
        {
            enemyModel.TakeDamage(damage);

            if (enemyModel.CurrentHealth <= 0 && !isDead)
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
            enemyView.gameObject.SetActive(false);
            enemyPool.ReturnItem(this);
        }

        public virtual void Reset()
        {
            enemyModel.Reset();
            isDead = false;
            _isPlayerDead = false;
            enemyStateMachine.ChangeState(EnemyStates.IDLE);
        }
    }
}