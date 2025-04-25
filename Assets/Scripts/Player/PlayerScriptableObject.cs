using UnityEngine;

namespace DragonBall.Player
{
    [CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "Player/PlayerScriptableObject")]
    public class PlayerScriptableObject : ScriptableObject
    {
        [Header("Stats")]
        public int PlayerHealth = 100;

        [Header("Movement")]
        public float MoveSpeed = 5f;

        [Header("Jump")]
        public float JumpSpeed = 10f;
        public float JumpHorizontalDampening = 0.9f;

        [Header("Vanish")]
        public float vanishRange = 5f;

        [Header("Dodge")]
        public float DodgeSpeed = 15f;
        public float DodgeDuration = 0.2f;
        public float DodgeCooldown = 0.5f;
    }
}