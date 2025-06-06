using UnityEngine;
using DragonBall.Core;
using DragonBall.Enemy.ParentMVC;
using DragonBall.Sound.SoundData;
using DragonBall.Sound.SoundUtilities;
using DragonBall.GameStrings;

namespace DragonBall.Enemy.KickEnemyMVC
{
    public class KickTypeEnemyView : BaseEnemyView
    {
        [Header("KickType Specific Attack Settings")]
        [SerializeField] private AnimationClip buuKickAnimation;

        protected override float GetAttackAnimationLength() => buuKickAnimation != null ? buuKickAnimation.length : 0.5f;

        protected override void PerformAttack()
        {
            if (baseEnemyController != null && baseEnemyController.IsPlayerDead) return;

            Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            Vector2 origin = (Vector2)transform.position + Vector2.Scale(direction, attackOffset);

            SoundManager.Instance.PlaySoundEffect(SoundType.KickTypeEnemyKick);

            var hits = Physics2D.CircleCastAll(origin, attackRadius, Vector2.zero);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag(GameString.PlayerTag))
                {
                    var playerController = GameService.Instance.playerService.PlayerController;
                    if (!playerController.PlayerModel.IsDead)
                        playerController.TakeDamage(baseEnemyController.BaseEnemyModel.AttackDamage);
                }
            }
        }
    }
}