using DragonBall.Player;
using UnityEngine;

namespace DragonBall.UI
{
    public class GameplayUIController
    {
        private readonly GameplayUIView gameplayUIView;
        private readonly PlayerModel playerModel;

        private float previousHealth;
        private int previousDragonBallCount;

        public GameplayUIController(GameplayUIView view, PlayerModel model)
        {
            gameplayUIView = view;
            playerModel = model;

            previousHealth = playerModel.CurrentHealth;
            previousDragonBallCount = playerModel.DragonBallCount;

            UpdateHealthDisplay();
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
    }
}