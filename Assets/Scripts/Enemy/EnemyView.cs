using UnityEngine;

namespace DragonBall.Enemy
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;

        private EnemyController controller;
        private Animator animator;
        private Rigidbody2D rb;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        public void SetController(EnemyController controllerToSet) => controller = controllerToSet;

        public void SetPosition(Vector3 position) => transform.position = position;

        public EnemyType GetEnemyType() => enemyType;

        public void SetVelocity(Vector2 velocity)
        {
            if (velocity.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (velocity.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            rb.linearVelocity = velocity;
        }

        public void SetRunning(bool isRunning) => animator.SetBool("isRunning", isRunning);

        public void SetFacingDirection(float directionX)
        {
            if (directionX > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (directionX < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}