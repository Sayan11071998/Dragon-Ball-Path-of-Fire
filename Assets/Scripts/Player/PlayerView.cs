using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace DragonBall.Player
{
    public class PlayerView : MonoBehaviour
    {
        [Header("Attack Settings")]
        [SerializeField] private Transform attackTransform;
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private LayerMask attackableLayer;
        [SerializeField] private Transform fireTransform;
        [SerializeField] private Transform kamehamehaTransform;

        [Header("Death Settings")]
        [SerializeField] private AnimationClip playerDeathClip;
        [SerializeField] private float deathClipDurationOffset = 0.5f;
        [SerializeField] private float flyAwayForceX = 5f;
        [SerializeField] private float flyAwayForceY = 2f;
        [SerializeField] private float flyAwayDuration = 0.3f;

        [Header("Animation Clips")]
        [SerializeField] private AnimationClip superSaiyanTransformClip;
        [SerializeField] private AnimationClip kamehamehaAnimationClip;

        [Header("Transformation Settings")]
        [SerializeField] private RuntimeAnimatorController normalAnimatorController;
        [SerializeField] private RuntimeAnimatorController superSaiyanAnimatorController;

        private PlayerController playerController;
        private Rigidbody2D rb;
        private CapsuleCollider2D capsuleCollider2D;
        private Animator animator;

        private float moveInput;
        private bool isJumping;
        private bool isFlying;
        private bool isVanishing;
        private bool isDodging;
        private bool isKicking;
        private bool isFiring;
        private bool isKamehameha;

        private bool isInputEnabled = true;
        private bool isSuperSaiyan = false;

        private Vector2 movementDirection;

        public Rigidbody2D Rigidbody => rb;
        public Animator Animator => animator;
        public Transform AttackTransform => attackTransform;
        public LayerMask AttackableLayer => attackableLayer;
        public float AttackRange => attackRange;
        public Transform FireTransform => fireTransform;
        public Transform KamehamehaTransform => kamehamehaTransform;

        public AnimationClip SuperSaiyanAnimationClip => superSaiyanTransformClip;
        public AnimationClip KamehamehaAnimationClip => kamehamehaAnimationClip;

        public bool IsSuperSaiyan => isSuperSaiyan;

        public float MoveInput => moveInput;
        public bool JumpInput => isJumping;
        public bool FlyInput => isFlying;
        public bool VanishInput => isVanishing;
        public bool DodgeInput => isDodging;
        public bool KickInput => isKicking;
        public bool FireInput => isFiring;
        public bool KamehamehaInput => isKamehameha;

        public Vector2 MovementDirection => movementDirection;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        public void SetPlayerController(PlayerController controller) => playerController = controller;

        public void OnMove(InputValue value)
        {
            if (isInputEnabled)
            {
                Vector2 inputVector = value.Get<Vector2>();
                moveInput = inputVector.x;
                movementDirection = inputVector;
            }
        }

        public void OnJump()
        {
            if (isInputEnabled)
                isJumping = true;
        }

        public void OnFlightToggle()
        {
            if (isInputEnabled)
                isFlying = true;
        }

        public void OnVanish()
        {
            if (isInputEnabled)
                isVanishing = true;
        }

        public void OnDodge()
        {
            if (isInputEnabled)
                isDodging = true;
        }

        public void OnKick()
        {
            if (isInputEnabled)
                isKicking = true;
        }

        public void OnFire()
        {
            if (isInputEnabled)
                isFiring = true;
        }

        public void OnKamehameha()
        {
            if (isInputEnabled)
                isKamehameha = true;
        }

        public void EnableInput()
        {
            isInputEnabled = true;
            moveInput = 0f;
            ResetAllInputs();
        }

        public void DisableInput()
        {
            isInputEnabled = false;
            moveInput = 0f;
            ResetAllInputs();
        }

        public void ResetJumpInput() => isJumping = false;
        public void ResetFlyInput() => isFlying = false;
        public void ResetVanishInput() => isVanishing = false;
        public void ResetDodgeInput() => isDodging = false;
        public void ResetKickInput() => isKicking = false;
        public void ResetFireInput() => isFiring = false;
        public void ResetKamehameha() => isKamehameha = false;

        public void ResetAllInputs()
        {
            isJumping = false;
            isFlying = false;
            isVanishing = false;
            isDodging = false;
            isKicking = false;
            isFiring = false;
            isKamehameha = false;
        }

        public bool IsTouchingGround() => capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        public void FlipCharacter(bool isFacingRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = isFacingRight ? 1 : -1;
            transform.localScale = localScale;
        }

        public void UpdateRunAnimation(bool isRunning) => animator.SetBool("isRunning", isRunning);
        public void UpdateJumpAnimation(bool isJumping) => animator.SetBool("isJumping", isJumping);
        public void UpdateFlightAnimation(bool isFlying) => animator.SetBool("isFlyingToggle", isFlying);
        public void SetDodgeAnimation(bool isDodging) => animator.SetBool("isDodging", isDodging);
        public void PlayKickAnimation() => animator.SetTrigger("isKickingTrigger");
        public void PlayFireAnimation() => animator.SetTrigger("isFiring");
        public void PlayKamehamehaAnimation() => animator.SetTrigger("isKamehameha");
        public void PlayDeathAnimation() => animator.SetTrigger("isDead");
        public void PlaySuperSaiyanTransformationAnimation() => animator.SetTrigger("isTranformingSuperSaiyan");

        public void StartFireCoroutine(float delay, Action onComplete) => StartCoroutine(FireAfterDelay(delay, onComplete));

        private IEnumerator FireAfterDelay(float delay, Action onComplete)
        {
            yield return new WaitForSeconds(delay);
            onComplete?.Invoke();
        }

        public void Damage(int amount)
        {
            if (playerController != null)
                playerController.TakeDamage(amount);
        }

        public IEnumerator DeathSequence()
        {
            PlayDeathAnimation();
            yield return new WaitForSeconds(0.1f);

            float directionX = transform.localScale.x > 0 ? -1 : 1;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(directionX * flyAwayForceX, flyAwayForceY), ForceMode2D.Impulse);
            yield return new WaitForSeconds(flyAwayDuration);

            yield return new WaitForSeconds(playerDeathClip.length + deathClipDurationOffset);

            Destroy(gameObject);
        }

        public void StopPlayerMovement()
        {
            rb.linearVelocity = Vector2.zero;
            ResetAllInputs();
        }

        public void TransformToSuperSaiyan()
        {
            if (isSuperSaiyan) return;

            if (superSaiyanAnimatorController != null)
                animator.runtimeAnimatorController = superSaiyanAnimatorController;

            isSuperSaiyan = true;
        }

        public void RevertToNormal()
        {
            if (!isSuperSaiyan) return;

            if (normalAnimatorController != null)
                animator.runtimeAnimatorController = normalAnimatorController;

            isSuperSaiyan = false;
        }
    }
}