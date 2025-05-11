namespace DragonBall.GameStrings
{
    public static class GameString
    {
        #region Tags
        public const string PlatformTag = "Platform";
        public const string PlayerTag = "Player";
        #endregion

        #region Layers
        public const string GroundLayer = "Ground";
        public const string PlatformLayer = "Platform";
        public const string ObstacleLayer = "Obstacle";
        #endregion

        #region Player
        public const string PlayerPrefabName = "Songoku";
        public const string PlayerAnimationRunBool = "isRunning";
        public const string PlayerAnimationJumpBool = "isJumping";
        public const string PlayerAnimationFlightBool = "isFlyingToggle";
        public const string PlayerAnimationDodgeBool = "isDodging";
        public const string PlayerAnimationKickTrigger = "isKickingTrigger";
        public const string PlayerAnimationFireTrigger = "isFiring";
        public const string PlayerAnimationKamekamehaTrigger = "isKamehameha";
        public const string PlayerAnimationDeathTrigger = "isDead";
        public const string PlayerAnimationTransformSuperSaiyanTrigger = "isTranformingSuperSaiyan";
        #endregion

        #region Enemy
        public const string EnemyAnimatorMoveBool = "isMoving";
        public const string EnemyAnimatorAttackBool = "isAttacking";
        public const string EnemyAnimatorHealthRegenerationBool = "isRegenerating";
        public const string EnemyAnimatorDeathBool = "isDead";
        #endregion

        #region UI
        public const string GameplayUIPrefabName = "GameplayUI";
        #endregion

        #region Scene
        public const string SceneFinalBoss = "Final Boss";
        #endregion
    }
}