using UnityEngine;

namespace DragonBall.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyView : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        public Transform Transform => transform;

        private EnemyController controller;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        public void Initialize(EnemyController controller)
        {
            this.controller = controller;
        }

        public void SetPosition(Vector3 position) => Transform.position = position;

        public void MoveTo(Vector3 target, float speed)
        {
            Animator.SetBool("isMoving", true);
            Vector3 direction = (target - Transform.position).normalized;
            Transform.position += direction * speed * Time.deltaTime;
        }

        public void StopMoving()
        {
            Animator.SetBool("isMoving", false);
        }

        public void Attack()
        {
            StopMoving();
            Animator.SetTrigger("isAttacking");
        }

        public void Die()
        {
            Animator.SetTrigger("isDeath");
        }
    }
}