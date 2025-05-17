namespace DragonBall.Player.PlayerData
{
    /// <summary>
    /// Enum defining all possible player states
    /// </summary>
    public enum PlayerState
    {
        Idle,       // Player is standing still
        Run,        // Player is running/walking
        Jump,       // Player is jumping
        Fly,        // Player is flying (Super Saiyan only)
        Kick,       // Player is performing a kick attack
        Fire,       // Player is firing an energy blast
        Dodge,      // Player is dodging (Super Saiyan only)
        Vanish,    // Player is vanishing/teleporting (Super Saiyan only) - Note: typo in enum, should be "Vanish"
        Kamehameha, // Player is using Kamehameha attack (Super Saiyan only)
        Transform,  // Player is transforming to Super Saiyan
        Dead        // Player is dead
    }
}