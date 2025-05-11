using UnityEngine;
using DragonBall.Enemy.EnemyData;
using DragonBall.Enemy.EnemyUtilities;
using DragonBall.Enemy.ParentMVC;
using DragonBall.Utilities;
using DragonBall.GameStrings;

namespace DragonBall.Enemy.EnemyState
{
    public abstract class BaseState : IState
    {
        protected BaseEnemyController baseEnemyController;
        protected EnemyStateMachine enemyStateMachine;
        protected Transform playerTransform;
        protected EnemyScriptableObject enemyScriptableObject;

        public BaseState(BaseEnemyController controllerToSet, EnemyStateMachine stateMachineToSet)
        {
            baseEnemyController = controllerToSet;
            enemyStateMachine = stateMachineToSet;
            enemyScriptableObject = controllerToSet.EnemyData;

            playerTransform = GameObject.FindGameObjectWithTag(GameString.PlayerTag)?.transform;
        }

        public virtual void OnStateEnter() { }

        public virtual void Update()
        {
            if (ShouldExitDueToHealth()) return;
            if (ShouldExitDueToPlayerDeath()) return;

            StateSpecificUpdate();
        }

        protected virtual void StateSpecificUpdate() { }

        protected bool ShouldExitDueToHealth()
        {
            if (baseEnemyController.IsDead || baseEnemyController.BaseEnemyView.IsDying)
            {
                if (baseEnemyController.IsDead && !baseEnemyController.BaseEnemyView.IsDying)
                    enemyStateMachine.ChangeState(EnemyStates.DEATH);
                return true;
            }
            return false;
        }

        protected bool ShouldExitDueToPlayerDeath()
        {
            if (baseEnemyController.IsPlayerDead || playerTransform == null)
            {
                if (baseEnemyController.IsPlayerDead)
                    baseEnemyController.BaseEnemyView.StopMovement();

                enemyStateMachine.ChangeState(EnemyStates.IDLE);
                return true;
            }
            return false;
        }

        public virtual void OnStateExit() { }

        protected virtual float GetDistanceToPlayer()
        {
            if (playerTransform == null)
                return float.MaxValue;

            return Vector2.Distance(baseEnemyController.BaseEnemyView.transform.position, playerTransform.position);
        }

        protected bool HasLineOfSightToPlayer(LayerMask? layerMask = null)
        {
            if (playerTransform == null)
                return false;

            Vector2 enemyPosition = baseEnemyController.BaseEnemyView.transform.position;
            Vector2 playerPosition = playerTransform.position;
            Vector2 direction = playerPosition - enemyPosition;
            float distance = direction.magnitude;

            LayerMask obstacleLayer = layerMask ?? LayerMask.GetMask(GameString.GroundLayer, GameString.PlatformLayer, GameString.ObstacleLayer);

            RaycastHit2D hit = Physics2D.Raycast(
                enemyPosition,
                direction.normalized,
                distance,
                obstacleLayer
            );

            return hit.collider == null;
        }
    }
}