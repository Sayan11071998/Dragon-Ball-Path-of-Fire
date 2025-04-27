using System.Collections.Generic;
using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyStateMachine
    {
        private IState currentState;
        public Dictionary<EnemyStateType, IState> states;
        private EnemyStateType currentEnemyStateEnum;
        private EnemyController enemyController;

        public EnemyStateMachine(EnemyController enemyController)
        {
            this.enemyController = enemyController;
            CreateStates(enemyController);
        }

        private void CreateStates(EnemyController enemyController)
        {
            states = new Dictionary<EnemyStateType, IState>()
            {
                { EnemyStateType.IDLE, new IdleState(enemyController, this) },
                { EnemyStateType.CHASE, new ChaseState(enemyController, this) },
                { EnemyStateType.ATTACK, new AttackState(enemyController, this) },
                { EnemyStateType.DIE, new DieState(enemyController, this) }
            };
        }

        public void ChangeState(EnemyStateType newState)
        {
            if (states.ContainsKey(newState))
            {
                currentEnemyStateEnum = newState;
                ChangeState(states[newState]);
            }
            else
            {
                Debug.LogError($"State {newState} doesn't exist in the enemy state machine.");
            }
        }

        private void ChangeState(IState newState)
        {
            currentState?.OnStateExit();
            currentState = newState;
            currentState?.OnStateEnter();
        }

        public void Update() => currentState?.Update();
    }
}