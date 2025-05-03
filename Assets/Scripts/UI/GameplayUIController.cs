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

            UpdateHealthDisplay();
        }

        public void Update()
        {
            if (playerModel == null) return;

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

            if (healthPercentage <= 0.30f)
                gameplayUIView.SetHealthBarDangerState(true);
            else
                gameplayUIView.SetHealthBarDangerState(false);
        }
    }
}