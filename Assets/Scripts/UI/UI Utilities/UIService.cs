using UnityEngine;
using System.Collections;
using DragonBall.GameStrings;
using DragonBall.Player.PlayerMVC;
using DragonBall.UI.UIController;
using DragonBall.UI.UIView;

namespace DragonBall.UI.UIUtilities
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
            gameplayUIView.name = GameString.GameplayUIPrefabName;
            gameplayUIController = new GameplayUIController(gameplayUIView, playerModel);
        }

        public void Update() => gameplayUIController?.Update();

        public void ShowNotification(System.Action onButtonPressed)
        {
            gameplayUIView.ActivateNotificationPanel();
            gameplayUIView.notificationButton.onClick.RemoveAllListeners();
            gameplayUIView.notificationButton.onClick.AddListener(() =>
            {
                gameplayUIView.notificationPanel.SetActive(false);
                onButtonPressed?.Invoke();
            });
        }

        public void ShowGameOver() => gameplayUIView.ShowGameOverPanel();

        public void ShowGameCompletePanel() => gameplayUIView.ShowGameCompletePanel();

        public IEnumerator ShowNotificationBeforeEnablingPlayer(PlayerController playerController)
        {
            bool isNotificationHandled = false;
            ShowNotification(() => isNotificationHandled = true);
            yield return new WaitUntil(() => isNotificationHandled);

            playerController.EnablePlayerController();
        }
    }
}