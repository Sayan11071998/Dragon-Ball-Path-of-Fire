using System.Collections.Generic;
using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyStateMachine
    {
        public Dictionary<EnemyStates, IState> states;

        private IState currentState;
        private EnemyStates currentEnemyStateEnum;
        private BaseEnemyController enemyController;
        private BaseEnemyView enemyView;

        public EnemyStateMachine(BaseEnemyController enemyControllerToSet)
        {
            enemyController = enemyControllerToSet;
            enemyView = enemyControllerToSet.BaseEnemyView;
            CreateStates(enemyControllerToSet);
        }

        private void CreateStates(BaseEnemyController enemyController)
        {
            states = new Dictionary<EnemyStates, IState>()
            {
                { EnemyStates.IDLE, new IdleState(enemyController, this) },
                { EnemyStates.RUNNING, new RunningState(enemyController, this) },
                { EnemyStates.ATTACK, new AttackState(enemyController, this) },
                { EnemyStates.DEATH, new DeathState(enemyController, this) }
            };
        }

        public void ChangeState(EnemyStates newState)
        {
            if (states.ContainsKey(newState))
            {
                if (currentEnemyStateEnum != newState || currentState == null)
                {
                    enemyView.ResetAllInputs();

                    currentEnemyStateEnum = newState;
                    ChangeState(states[newState]);
                }
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
        public EnemyStates GetCurrentPlayerState() => currentEnemyStateEnum;
        public Dictionary<EnemyStates, IState> GetStates() => states;
    }
}