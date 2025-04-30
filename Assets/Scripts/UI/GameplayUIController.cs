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
            // Only update the UI if health has changed
            if (playerModel.CurrentHealth != previousHealth)
            {
                UpdateHealthDisplay();
                previousHealth = playerModel.CurrentHealth;
            }
        }

        private void UpdateHealthDisplay()
        {
            float healthPercentage = playerModel.CurrentHealth / playerModel.MaxHealth;
            gameplayUIView.UpdateHealthBar(healthPercentage);

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