using DragonBall.Core;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyController : IDamageable
    {
        private EnemyModel enemyModel;
        private EnemyView enemyView;
        private EnemyStateMachine stateMachine;
        private Transform playerTransform;

        public EnemyModel EnemyModel => enemyModel;
        public EnemyView EnemyView => enemyView;
        public bool IsDead => enemyModel.IsDead;

        public EnemyController(EnemyModel model, EnemyView view)
        {
            enemyModel = model;
            enemyView = view;
            playerTransform = GameService.Instance.playerService.PlayerPrefab.transform;

            stateMachine = new EnemyStateMachine(this);
            stateMachine.ChangeState(EnemyStateType.IDLE);
        }

        public void Update()
        {
            stateMachine.Update();
        }

        public void Damage(float damageAmount)
        {
            enemyModel.TakeDamage((int)damageAmount);
            Debug.Log($"Enemy took {damageAmount} damage! Remaining health: {enemyModel.Health}");

            if (enemyModel.IsDead)
            {
                stateMachine.ChangeState(EnemyStateType.DIE);
            }
        }

        public void ReturnToPool()
        {
            GameService.Instance.enemyService.ReturnEnemyToPool(this);
        }

        public float GetDistanceToPlayer()
        {
            return Vector2.Distance(enemyView.transform.position, playerTransform.position);
        }

        public Vector2 GetDirectionToPlayer()
        {
            return (playerTransform.position - enemyView.transform.position).normalized;
        }
    }
}