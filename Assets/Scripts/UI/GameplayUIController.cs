using DragonBall.Player;
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

        public GameplayUIController(GameplayUIView view, PlayerModel model)
        {
            gameplayUIView = view;
            playerModel = model;

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

            if (!Mathf.Approximately(playerModel.CurrentHealth, previousHealth))
            {
                UpdateHealthDisplay();
                previousHealth = playerModel.CurrentHealth;
            }

            if (!Mathf.Approximately(playerModel.CurrentStamina, previousStamina))
            {
                UpdateStaminaDisplay();
                previousStamina = playerModel.CurrentStamina;
            }

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