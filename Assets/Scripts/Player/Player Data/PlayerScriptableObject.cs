using UnityEngine;

namespace DragonBall.Player.PlayerData
{
    [CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "Player/PlayerScriptableObject")]
    public class PlayerScriptableObject : ScriptableObject
    {
        [Header("Stats")]
        public float PlayerHealth = 100f;
        public float PlayerStamina = 100f;

        [Header("Stamina")]
        public float StaminaRegenRate = 10f;
        public float KamehamehaStaminaCost = 30f;

        [Header("Movement")]
        public float MoveSpeed = 5f;

        [Header("Jump")]
        public float JumpSpeed = 10f;
        public float JumpHorizontalDampening = 0.9f;

        [Header("Fly")]
        public float FlySpeed = 4f;

        [Header("Vanish")]
        public float VanishRange = 5f;

        [Header("Dodge")]
        public float DodgeSpeed = 15f;
        public float DodgeDuration = 0.2f;
        public float DodgeCooldown = 0.5f;

        [Header("Kick Attack")]
        public int KickAttackPower = 10;
        public float KickAttackRange = 1.5f;
        public float KickAttackCooldown = 0.5f;

        [Header("Fire Attack")]
        public float FireCooldown = 0.5f;

        [Header("Super Saiyan Transformation")]
        public int DragonBallsRequiredForSuperSaiyan = 5;
        public float SuperSaiyanHealthMultiplier = 2f;
        public float SuperSaiyanStaminaMultiplier = 2f;
        public float SuperSaiyanSpeedMultiplier = 1.5f;
        public float SuperSaiyanPowerMultiplier = 2f;
    }
}