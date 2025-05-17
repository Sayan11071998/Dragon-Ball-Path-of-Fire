using DragonBall.Player.PlayerData;
using DragonBall.Utilities;

namespace DragonBall.Player.PlayerUtilities
{
    public interface IPlayerState : IState
    {
        public PlayerState GetStateType();
    }
}