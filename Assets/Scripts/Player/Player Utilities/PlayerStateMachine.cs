using System.Collections.Generic;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerStates;
using DragonBall.Utilities;

namespace DragonBall.Player.PlayerUtilities
{
    public class PlayerStateMachine
    {
        private Dictionary<PlayerState, IPlayerState> states;
        private PowerStateManager powerStateManager;
        private IPlayerState currentState;
        private PlayerController playerController;

        public PlayerStateMachine(PlayerController controller)
        {
            playerController = controller;
            InitializeStates();
            powerStateManager = new PowerStateManager(controller);

            ChangeState(PlayerState.Idle);
        }

        private void InitializeStates()
        {
            states = new Dictionary<PlayerState, IPlayerState>
            {
                { PlayerState.Idle, new IdleState(playerController, this) },
                { PlayerState.Run, new RunState(playerController, this) },
                { PlayerState.Jump, new JumpState(playerController, this) },
                { PlayerState.Fall, new FallState(playerController, this) },
                { PlayerState.Fly, new FlyState(playerController, this) },
                { PlayerState.Dead, new DeadState(playerController, this) }
            };
        }

        public void ChangeState(PlayerState newStateType)
        {
            if (currentState != null && currentState.GetStateType() == newStateType)
                return;

            IPlayerState newState = states[newStateType];

            currentState?.OnStateExit();
            currentState = newState;
            currentState?.OnStateEnter();
        }

        public void Update()
        {
            powerStateManager.Update();

            if (!powerStateManager.IsTransforming)
                currentState?.Update();
        }

        public bool CanFly() => powerStateManager.CanFly();

        public bool CanVanish() => powerStateManager.CanVanish();

        public bool CanUseKamehameha() => powerStateManager.CanUseKamehameha();

        public bool CanDodge() => true;

        public PlayerState GetCurrentStateType() => currentState.GetStateType();

        public PlayerPowerState GetCurrentPowerState() => powerStateManager.CurrentPowerState;
    }
}