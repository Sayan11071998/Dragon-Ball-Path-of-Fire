using UnityEngine;

namespace DragonBall.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyView : MonoBehaviour
    {
        private Animator animator;
        // private Rigidbody2D rb;
        // private CapsuleCollider2D capsuleCollider2D;
        private EnemyController controller;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetController(EnemyController controllerToSet) => controller = controllerToSet;

        // public void Initialize(EnemyController controller)
        // {
        //     this.controller = controller;
        // }

        // public void SetPosition(Vector3 position) => Transform.position = position;

        // public void MoveTo(Vector3 target, float speed)
        // {
        //     Animator.SetBool("isMoving", true);
        //     Vector3 direction = (target - Transform.position).normalized;
        //     Transform.position += direction * speed * Time.deltaTime;
        // }

        // public void StopMoving()
        // {
        //     Animator.SetBool("isMoving", false);
        // }

        // public void Attack()
        // {
        //     StopMoving();
        //     Animator.SetTrigger("isAttacking");
        // }

        // public void Die()
        // {
        //     Animator.SetTrigger("isDeath");
        // }
    }
}