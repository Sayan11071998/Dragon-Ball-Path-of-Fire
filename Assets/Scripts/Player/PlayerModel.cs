namespace DragonBall.Player
{
    public class PlayerModel
    {
        public int Health { get; private set; }

        public PlayerModel(int _Health)
        {
            Health = _Health;
        }
    }
}