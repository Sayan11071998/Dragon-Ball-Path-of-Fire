using UnityEngine;

namespace DragonBall.Enemy
{
    public abstract class BaseEnemyState : IState
    {
        protected EnemyController enemyController;
        protected EnemyStateMachine stateMachine;
        protected EnemyModel enemyModel;
        protected EnemyView enemyView;

        public BaseEnemyState(EnemyController controller, EnemyStateMachine stateMachine)
        {
            this.enemyController = controller;
            this.stateMachine = stateMachine;
            this.enemyModel = controller.EnemyModel;
            this.enemyView = controller.EnemyView;
        }

        public abstract void OnStateEnter();
        public abstract void Update();
        public abstract void OnStateExit();

        protected void FacePlayer(Vector2 directionToPlayer)
        {
            bool isFacingRight = directionToPlayer.x > 0;
            enemyView.FlipCharacter(isFacingRight);
        }
    }
}