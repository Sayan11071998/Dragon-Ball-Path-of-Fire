using System.Collections;
using DragonBall.Bullet;
using DragonBall.Core;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class FinalBossTypeEnemyView : FlyingTypeEnemyView
    {
        [Header("Final Boss Specific Settings")]
        [SerializeField] private float bossFloatAmplitude = 0.7f;
        [SerializeField] private float bossFloatSpeed = 0.8f;
        [SerializeField] private BulletType bossBulletType = BulletType.EnemyGuidedPowerBall;

        protected override float FloatAmplitudeValue => bossFloatAmplitude;
        protected override float FloatSpeedValue => bossFloatSpeed;
        protected override BulletType EnemyBulletType => bossBulletType;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void StartDeathAnimation()
        {
            if (isDying) return;

            base.StartDeathAnimation();
        }

        public void SpecialBossAttack()
        {
        }

        protected override void PerformAttack()
        {
            base.PerformAttack();
        }
    }
}