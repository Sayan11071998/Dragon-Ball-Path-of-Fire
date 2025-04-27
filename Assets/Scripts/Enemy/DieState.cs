using UnityEngine;

namespace DragonBall.Enemy
{
    public class DieState : BaseEnemyState
    {
        private const float DeathAnimationDuration = 1.5f;

        public DieState(EnemyController controller, EnemyStateMachine stateMachine) : base(controller, stateMachine) { }

        public override void OnStateEnter()
        {
            Debug.Log("Enemy entered DIE state");
            enemyView.SetVelocity(Vector2.zero);
            enemyView.StartCoroutine(enemyView.PlayDeathSequence(DeathAnimationDuration));

            // Return to pool after death animation completes
            enemyView.StartCoroutine(ReturnToPoolAfterDelay(DeathAnimationDuration));
        }

        private System.Collections.IEnumerator ReturnToPoolAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            enemyController.ReturnToPool();
        }

        public override void Update() { }

        public override void OnStateExit() { }
    }
}