using System.Collections.Generic;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerStates;
using DragonBall.Utilities;

namespace DragonBall.Player.PlayerUtilities
{
    public class PlayerStateMachine
    {
        public Dictionary<PlayerState, IState> states;

        private IState currentPlayerState;
        private PlayerState currentPlayerStateEnum;
        private PlayerController playerController;
        private PlayerView playerView;

        public PlayerStateMachine(PlayerController controllerToSet)
        {
            playerController = controllerToSet;
            playerView = controllerToSet.PlayerView;

            CreateStates(controllerToSet);
        }

        private void CreateStates(PlayerController playerController)
        {
            states = new Dictionary<PlayerState, IState>()
            {
                { PlayerState.NORMAL, new NormalState(playerController, this) },
                { PlayerState.SUPER_SAIYAN, new SuperSaiyanState(playerController, this) }
            };
        }

        public void ChangeState(PlayerState newState)
        {
            if (states.ContainsKey(newState))
            {
                playerView.ResetAllInputs();

                currentPlayerStateEnum = newState;
                ChangeState(states[newState]);
            }
        }

        private void ChangeState(IState newState)
        {
            currentPlayerState?.OnStateExit();
            currentPlayerState = newState;
            currentPlayerState?.OnStateEnter();
        }

        public void Update() => currentPlayerState?.Update();

        public IState GetCurrentState() => currentPlayerState;
        public PlayerState GetCurrentPlayerState() => currentPlayerStateEnum;
        public Dictionary<PlayerState, IState> GetStates() => states;
    }
}