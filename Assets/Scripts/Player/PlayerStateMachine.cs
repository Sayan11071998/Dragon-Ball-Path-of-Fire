using System.Collections.Generic;
using DragonBall.Utilities;
using DragonBall.Core;
using UnityEngine;

namespace DragonBall.Player
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

                if (GameService.Instance != null && GameService.Instance.cameraService != null)
                {
                    GameService.Instance.cameraService.HandlePlayerStateChange(newState);
                }
            }
            else
            {
                Debug.LogError($"State {newState} doesn't exist in the state machine.");
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