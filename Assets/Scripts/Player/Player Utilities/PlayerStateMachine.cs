using System.Collections.Generic;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerStates;
using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Player.PlayerUtilities
{
    public class PlayerStateMachine
    {
        private Dictionary<PlayerState, IState> states;

        private IState currentPlayerState;
        private PlayerState currentPlayerStateEnum;
        private PlayerController playerController;
        private bool isInitialized = false;

        public PlayerStateMachine(PlayerController controllerToSet)
        {
            playerController = controllerToSet;

            InitializeStates();
            ChangeState(PlayerState.Idle);
            isInitialized = true;
        }

        private void InitializeStates()
        {
            states = new Dictionary<PlayerState, IState>()
            {
                { PlayerState.Idle, new IdleState(playerController, this) },
                { PlayerState.Run, new RunState(playerController, this) },
                { PlayerState.Jump, new JumpState(playerController, this) },
                { PlayerState.Fly, new FlyState(playerController, this) },
                { PlayerState.Transform, new TransformState(playerController, this) },

                // { PlayerState.Kick, new KickState(playerController, this) },
                // { PlayerState.Fire, new FireState(playerController, this) },
                // { PlayerState.Dodge, new DodgeState(playerController, this) },
                // { PlayerState.Vanish, new VanishState(playerController, this) },
                // { PlayerState.Kamehameha, new KamehamehaState(playerController, this) },
                // { PlayerState.Transform, new TransformState(playerController, this) },
                // { PlayerState.Dead, new DeadState(playerController, this) }
            };
        }

        public void ChangeState(PlayerState newState)
        {
            Debug.Log($"Changing state from {currentPlayerStateEnum} to {newState}");

            if (!states.ContainsKey(newState))
            {
                Debug.LogWarning($"Attempted to change to non-existent state: {newState}");
                return;
            }

            if (currentPlayerStateEnum == newState && isInitialized) return;

            currentPlayerStateEnum = newState;
            ChangeState(states[newState]);
        }

        private void ChangeState(IState newState)
        {
            if (currentPlayerState != null)
                currentPlayerState.OnStateExit();

            currentPlayerState = newState;
            currentPlayerState.OnStateEnter();
        }

        public void Update()
        {
            if (currentPlayerState != null)
                currentPlayerState.Update();
            else if (isInitialized)
                Debug.LogError("Current state is null in PlayerStateMachine.Update()");
        }

        public IState GetCurrentState() => currentPlayerState;

        public PlayerState GetCurrentPlayerState() => currentPlayerStateEnum;

        public Dictionary<PlayerState, IState> GetStates() => states;
    }
}