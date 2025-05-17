using UnityEngine;
using System.Collections;
using DragonBall.Core;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Sound.SoundData;
using DragonBall.Sound.SoundUtilities;
using DragonBall.VFX;
using DragonBall.Bullet.BulletData;
using DragonBall.Enemy.EnemyUtilities;

namespace DragonBall.Player.PlayerMVC
{
    public class PlayerController
    {
        private PlayerModel playerModel;
        private PlayerView playerView;

        public PlayerModel PlayerModel => playerModel;
        public PlayerView PlayerView => playerView;

        private bool isInputEnabled = true;
        private bool isSuperSaiyanMode = false;

        public PlayerController(PlayerModel _playerModel, PlayerView _playerView, PlayerState initialState = PlayerState.NORMAL)
        {
            playerModel = _playerModel;
            playerView = _playerView;

            playerView.SetPlayerController(this);
            
            // Handle initial state
            if (initialState == PlayerState.SUPER_SAIYAN)
                StartSuperSaiyanTransformation();
        }

        public void Update()
        {
            if (playerModel.IsDead) return;

            HandleGroundCheck();

            if (isInputEnabled)
                HandleMovement();

            // Handle various player abilities
            playerModel.RegenerateStamina(Time.deltaTime);
            
            // Always available abilities
            HandleKick();
            HandleFire();
            
            if (isSuperSaiyanMode)
            {
                // Super Saiyan abilities
                HandleFlight();
                HandleDodge();
                HandleVanish();
                HandleKamehameha();
            }
            else
            {
                // Normal abilities
                HandleJump();
                
                // Check for transformation
                if (playerModel.DragonBallCount >= playerModel.DragonBallsRequiredForSuperSaiyan)
                {
                    StartSuperSaiyanTransformation();
                }
            }
            
            // Update animations
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            float moveInput = playerView.MoveInput;
            playerView.UpdateRunAnimation(Mathf.Abs(moveInput) > 0.1f && !playerModel.IsDodging);
            playerView.UpdateJumpAnimation(!playerModel.IsGrounded);
            playerView.UpdateDodgeAnimation(playerModel.IsDodging);
        }

        private void HandleGroundCheck()
        {
            playerModel.IsGrounded = playerView.IsTouchingGround();

            if (playerModel.IsGrounded)
                playerModel.JumpCount = 0;
        }

        private void HandleMovement()
        {
            float moveInput = playerView.MoveInput;
            Vector2 moveDirection = playerView.MovementDirection;

            if (playerModel.IsDodging) return;

            var velocity = playerView.Rigidbody.linearVelocity;

            if (playerModel.IsFlying)
            {
                if (Mathf.Abs(moveDirection.x) > 0.1f)
                    velocity.x = moveDirection.x * playerModel.MoveSpeed;
                else
                    velocity.x = 0;

                if (Mathf.Abs(moveDirection.y) > 0.1f)
                    velocity.y = moveDirection.y * playerModel.FlySpeed;
                else
                    velocity.y = 0;
            }
            else
            {
                velocity.x = moveInput * playerModel.MoveSpeed;
            }

            playerView.Rigidbody.linearVelocity = velocity;

            if (moveDirection.x > 0 && !playerModel.IsFacingRight)
            {
                playerModel.IsFacingRight = true;
                playerView.FlipCharacter(true);
            }
            else if (moveDirection.x < 0 && playerModel.IsFacingRight)
            {
                playerModel.IsFacingRight = false;
                playerView.FlipCharacter(false);
            }
        }

        public void HandleJump()
        {
            if (!playerView.JumpInput) return;

            var velocity = playerView.Rigidbody.linearVelocity;

            if (playerModel.IsGrounded)
            {
                velocity.y = playerModel.JumpSpeed;
                velocity.x *= playerModel.JumpHorizontalDampening;
                playerModel.JumpCount++;
                SoundManager.Instance.PlaySoundEffect(SoundType.GokuJump);
            }
            else if (!playerModel.IsGrounded && playerModel.JumpCount < 1)
            {
                velocity.y = playerModel.JumpSpeed;
                velocity.x *= playerModel.JumpHorizontalDampening;
                GameService.Instance.vFXService.PlayVFXAtPosition(VFXType.JumpEffect, playerView.transform.position);
                SoundManager.Instance.PlaySoundEffect(SoundType.GokuJump);
                playerModel.JumpCount++;
            }
            playerView.Rigidbody.linearVelocity = velocity;
            playerView.ResetJumpInput();
        }

        public void HandleFlight()
        {
            if (!playerView.FlyInput) return;

            playerModel.IsFlying = !playerModel.IsFlying;
            playerView.Rigidbody.linearVelocity = Vector2.zero;
            playerView.ResetMovementDirection();

            if (playerModel.IsFlying)
            {
                playerView.UpdateFlightAnimation(true);
                playerView.Rigidbody.gravityScale = 0f;
                playerView.StartFlightSound();
            }
            else
            {
                playerView.UpdateFlightAnimation(false);
                playerView.Rigidbody.gravityScale = 1f;
                playerView.StopFlightSound();
            }

            playerView.ResetFlyInput();
        }
        
        private void HandleKick()
        {
            if (playerModel.IsDead || !playerModel.IsGrounded || !playerView.KickInput) return;

            if (playerModel.IsKickOnCooldown)
            {
                playerView.ResetKickInput();
                return;
            }

            playerModel.LastKickTime = Time.time;
            playerView.PlayKickAnimation();
            PerformKickAttack();
            playerView.ResetKickInput();
        }

        private void PerformKickAttack()
        {
            Vector2 origin = playerView.AttackTransform.position;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, playerModel.KickAttackRange, Vector2.zero, 0f);

            if (!playerModel.IsFlying)
                SoundManager.Instance.PlaySoundEffect(SoundType.GokuKick);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent<IDamageable>(out var target))
                    target.Damage(playerModel.KickAttackPower);
            }
        }

        private void HandleFire()
        {
            if (playerModel.IsDead || !playerView.FireInput) return;

            if (playerModel.IsFireOnCooldown)
            {
                playerView.ResetFireInput();
                return;
            }

            playerModel.LastFireTime = Time.time;
            playerView.PlayFireAnimation();
            FireBullet();
            playerView.ResetFireInput();
        }

        private void FireBullet()
        {
            Vector2 position = playerView.FireTransform.position;
            Vector2 direction = playerModel.IsFacingRight ? Vector2.right : Vector2.left;
            BulletType bulletType = isSuperSaiyanMode ? 
                BulletType.PlayerSuperSaiyanPowerBall : BulletType.PlayerNormalPowerBall;
                
            GameService.Instance.bulletService.FireBullet(bulletType, position, direction);
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuFire);
        }

        private void HandleDodge()
        {
            if (playerModel.IsDead)
            {
                playerView.ResetDodgeInput();
                return;
            }

            if (playerView.DodgeInput && playerModel.IsGrounded && Time.time > playerModel.LastDodgeTime + playerModel.DodgeCooldown)
            {
                playerModel.IsDodging = true;
                playerModel.DodgeEndTime = Time.time + playerModel.DodgeDuration;
                playerModel.LastDodgeTime = Time.time;
                Vector2 dir = playerModel.IsFacingRight ? Vector2.left : Vector2.right;
                playerView.Rigidbody.linearVelocity = new Vector2(dir.x * playerModel.DodgeSpeed, playerView.Rigidbody.linearVelocity.y);
                SoundManager.Instance.PlaySoundEffect(SoundType.GokuDodge);
                playerView.UpdateDodgeAnimation(true);
            }

            playerView.ResetDodgeInput();

            if (playerModel.IsDodging && Time.time > playerModel.DodgeEndTime)
            {
                playerModel.IsDodging = false;
                playerView.UpdateDodgeAnimation(false);
            }
        }

        private void HandleVanish()
        {
            if (playerModel.IsDead || !playerView.VanishInput) return;

            Vector2 originalPosition = playerView.transform.position;
            Vector2 randomOffset = Random.insideUnitCircle * playerModel.VanishRange;

            if (randomOffset.y < 0)
                randomOffset.y = Mathf.Abs(randomOffset.y);

            GameService.Instance.vFXService.PlayVFXAtPosition(VFXType.VanishEffect, originalPosition);
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuVanish);
            Vector2 newPosition = originalPosition + randomOffset;
            playerView.transform.position = new Vector3(newPosition.x, newPosition.y, playerView.transform.position.z);
            playerView.ResetVanishInput();
        }

        private void HandleKamehameha()
        {
            if (playerModel.IsDead || !playerView.KamehamehaInput) return;

            if (!playerModel.HasEnoughStaminaForKamehameha)
            {
                playerView.ResetKamehameha();
                return;
            }

            if (playerModel.UseStaminaForKamehameha())
            {
                AnimationClip kamehamehaClip = playerView.KamehamehaAnimationClip;
                playerView.PlayKamehamehaAnimation();
                SoundManager.Instance.PlaySoundEffect(SoundType.Kamekameha);
                playerView.StartFireCoroutine(kamehamehaClip.length, FireKamehameha);
            }

            playerView.ResetKamehameha();
        }

        private void FireKamehameha()
        {
            Vector2 position = playerView.KamehamehaTransform.position;
            Vector2 direction = playerModel.IsFacingRight ? Vector2.right : Vector2.left;
            GameService.Instance.bulletService.FireBullet(BulletType.PlayerKamehamehaPowerBall, position, direction);
        }

        public void CollectDragonBall()
        {
            playerModel.IncrementDragonBallCount();
            SoundManager.Instance.PlaySoundEffect(SoundType.DragonBallCollect);
        }

        public void TakeDamage(float damage)
        {
            if (playerModel.IsDead) return;

            playerModel.TakeDamage(damage);
            GameService.Instance.cameraShakeService.ShakeCamera(0.1f, 0.5f);
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuTakeDamage);

            if (playerModel.IsDead)
                playerView.StartCoroutine(playerView.DeathSequence());
        }

        public bool DisablePlayerController()
        {
            isInputEnabled = false;
            playerView.DisableInput();

            var velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = 0;
            playerView.Rigidbody.linearVelocity = velocity;

            return isInputEnabled;
        }

        public bool EnablePlayerController()
        {
            var velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = 0;
            playerView.Rigidbody.linearVelocity = velocity;

            isInputEnabled = true;
            playerView.EnableInput();
            return isInputEnabled;
        }
        
        public void StartSuperSaiyanTransformation()
        {
            DisablePlayerController();
            playerView.StopPlayerMovement();
            playerView.PlaySuperSaiyanTransformationAnimation();
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuSuperSaiyanTransform);
            playerModel.ApplySuperSaiyanBuffs();
            playerView.StartCoroutine(WaitForSuperSaiyanTransformation());
        }
        
        private IEnumerator WaitForSuperSaiyanTransformation()
        {
            AnimationClip transformClip = playerView.SuperSaiyanAnimationClip;
            yield return new WaitForSeconds(transformClip.length * 0.8f);

            playerView.TransformToSuperSaiyan();
            isSuperSaiyanMode = true;

            yield return new WaitForSeconds(transformClip.length * 0.2f);

            playerView.StopPlayerMovement();
            yield return new WaitForSeconds(0.1f);

            bool isNotificationHandled = false;
            GameService.Instance.uiService.ShowNotification(() => isNotificationHandled = true);
            yield return new WaitUntil(() => isNotificationHandled);

            EnablePlayerController();
        }
        
        public void RevertFromSuperSaiyan()
        {
            isSuperSaiyanMode = false;
            playerModel.RemoveSuperSaiyanBuffs();
            playerView.RevertToNormal();

            if (playerModel.IsFlying)
            {
                playerModel.IsFlying = false;
                playerView.StopFlightSound();
            }
        }
    }
}