using System.Collections.Generic;
using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Player
{
    public class PlayerStateMachine
    {
        private IState currentState;
        public Dictionary<PlayerState, IState> states;
        private PlayerState currentPlayerStateEnum;
        private PlayerController playerController;
        private PlayerView playerView;

        public PlayerStateMachine(PlayerController playerController)
        {
            this.playerController = playerController;
            this.playerView = playerController.PlayerView;
            CreateStates(playerController);
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
            else
            {
                Debug.LogError($"State {newState} doesn't exist in the state machine.");
            }
        }

        private void ChangeState(IState newState)
        {
            currentState?.OnStateExit();
            currentState = newState;
            currentState?.OnStateEnter();
        }

        public void Update() => currentState?.Update();

        public IState GetCurrentState() => currentState;
        public PlayerState GetCurrentPlayerState() => currentPlayerStateEnum;
        public Dictionary<PlayerState, IState> GetStates() => states;
    }
}