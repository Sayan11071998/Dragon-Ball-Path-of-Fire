using DragonBall.Player;
using DragonBall.Player.PlayerMVC;
using UnityEngine;

namespace DragonBall.UI
{
    public class GameplayUIController
    {
        private readonly GameplayUIView gameplayUIView;
        private readonly PlayerModel playerModel;

        private float previousHealth;
        private float previousStamina;
        private int previousDragonBallCount;

        public GameplayUIController(GameplayUIView _gameplayUIView, PlayerModel _playerModel)
        {
            gameplayUIView = _gameplayUIView;
            playerModel = _playerModel;

            previousHealth = playerModel.CurrentHealth;
            previousStamina = playerModel.CurrentStamina;
            previousDragonBallCount = playerModel.DragonBallCount;

            UpdateHealthDisplay();
            UpdateStaminaDisplay();
            gameplayUIView.UpdateDragonBallCount(previousDragonBallCount);
        }

        public void Update()
        {
            if (playerModel == null) return;

            CheckHealthChange();
            CheckStaminaChange();
            CheckDragonBallCountChange();
        }

        private void CheckHealthChange()
        {
            if (!Mathf.Approximately(playerModel.CurrentHealth, previousHealth))
            {
                UpdateHealthDisplay();
                previousHealth = playerModel.CurrentHealth;
            }
        }

        private void CheckStaminaChange()
        {
            if (!Mathf.Approximately(playerModel.CurrentStamina, previousStamina))
            {
                UpdateStaminaDisplay();
                previousStamina = playerModel.CurrentStamina;
            }
        }

        private void CheckDragonBallCountChange()
        {
            if (playerModel.DragonBallCount != previousDragonBallCount)
            {
                gameplayUIView.UpdateDragonBallCount(playerModel.DragonBallCount);
                previousDragonBallCount = playerModel.DragonBallCount;
            }
        }

        private void UpdateHealthDisplay()
        {
            float healthPercentage = playerModel.CurrentHealth / playerModel.MaxHealth;
            gameplayUIView.UpdateHealthBar(healthPercentage);
            gameplayUIView.SetHealthBarDangerState(healthPercentage <= 0.30f);
        }

        private void UpdateStaminaDisplay()
        {
            float staminaPercentage = playerModel.CurrentStamina / playerModel.MaxStamina;
            gameplayUIView.UpdateStaminaBar(staminaPercentage);
            gameplayUIView.SetStaminaBarDangerState(staminaPercentage <= 0.30f);
        }
    }
}