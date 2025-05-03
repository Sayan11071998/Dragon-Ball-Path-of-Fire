using DragonBall.Player;
using UnityEngine;

namespace DragonBall.UI
{
    public class GameplayUIController
    {
        private GameplayUIView gameplayUIView;
        private PlayerModel playerModel;

        private float previousHealth;

        public GameplayUIController(GameplayUIView view, PlayerModel model)
        {
            gameplayUIView = view;
            playerModel = model;
            previousHealth = playerModel.CurrentHealth;

            // Initialize the health bar with current values
            UpdateHealthDisplay();
        }

        public void Update()
        {
            // Check if player model is valid
            if (playerModel == null) return;

            // Only update the UI if health has changed
            if (!Mathf.Approximately(playerModel.CurrentHealth, previousHealth))
            {
                UpdateHealthDisplay();
                previousHealth = playerModel.CurrentHealth;
            }
        }

        private void UpdateHealthDisplay()
        {
            float healthPercentage = playerModel.CurrentHealth / playerModel.MaxHealth;
            gameplayUIView.UpdateHealthBar(healthPercentage);

            // Change health bar color when health is low (25% or less)
            if (healthPercentage <= 0.25f)
            {
                gameplayUIView.SetHealthBarDangerState(true);
            }
            else
            {
                gameplayUIView.SetHealthBarDangerState(false);
            }
        }
    }
}