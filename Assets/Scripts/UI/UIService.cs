using UnityEngine;
using DragonBall.Player;

namespace DragonBall.UI
{
    public class UIService
    {
        private GameplayUIController gameplayUIController;
        private GameplayUIView gameplayUIView;
        private PlayerModel playerModel;

        public UIService(GameplayUIView gameplayUIViewPrefab, PlayerModel playerModel)
        {
            this.playerModel = playerModel;

            gameplayUIView = Object.Instantiate(gameplayUIViewPrefab);
            gameplayUIView.name = "GameplayUI";

            gameplayUIController = new GameplayUIController(gameplayUIView, playerModel);
        }

        public void Update()
        {
            if (gameplayUIController != null)
                gameplayUIController.Update();
        }
    }
}