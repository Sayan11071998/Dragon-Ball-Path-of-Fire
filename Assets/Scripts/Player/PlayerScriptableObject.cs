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
        public float JumpSpeed = 10f;
    }
}