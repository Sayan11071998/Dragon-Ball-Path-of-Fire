using UnityEngine;
using UnityEngine.UI;
using DragonBall.Core;
using TMPro;
using UnityEngine.SceneManagement;

namespace DragonBall.UI
{
    public class GameplayUIView : MonoBehaviour
    {
        [Header("Player Info Panel")]
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider staminaSlider;

        [SerializeField] private Image healthBarFill;
        [SerializeField] private Image staminaBarFill;

        [SerializeField] private TextMeshProUGUI dragonBallCountText;

        [SerializeField] private Color normalHealthColor = Color.green;
        [SerializeField] private Color normalStaminaColor = Color.yellow;
        [SerializeField] private Color dangerHealthColor = Color.red;
        [SerializeField] private Color dangerStaminaColor = Color.gray;

        [Header("Notification Panel")]
        [SerializeField] public GameObject notificationPanel;
        [SerializeField] public Button notificationButton;

        [Header("Game Over Panel")]
        [SerializeField] public GameObject gameOverPanel;
        [SerializeField] public Button restartButton;
        [SerializeField] public Button exitButton;

        private void Start()
        {
            if (GameService.Instance?.playerService != null)
            {
                var playerModel = GameService.Instance.playerService.PlayerController.PlayerModel;

                healthSlider.maxValue = playerModel.MaxHealth;
                healthSlider.value = playerModel.CurrentHealth;

                staminaSlider.maxValue = playerModel.MaxStamina;
                staminaSlider.value = playerModel.CurrentStamina;

                notificationPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                UpdateDragonBallCount(playerModel.DragonBallCount);
            }

            if (restartButton != null)
                restartButton.onClick.AddListener(RestartGame);

            if (exitButton != null)
                exitButton.onClick.AddListener(ExitToMainMenu);
        }

        public void UpdateHealthBar(float healthPercentage) => healthSlider.value = healthPercentage * healthSlider.maxValue;
        public void UpdateStaminaBar(float staminaPercentage) => staminaSlider.value = staminaPercentage * staminaSlider.maxValue;

        public void SetHealthBarDangerState(bool isDanger)
        {
            if (healthBarFill != null)
                healthBarFill.color = isDanger ? dangerHealthColor : normalHealthColor;
        }

        public void SetStaminaBarDangerState(bool isDanger)
        {
            if (staminaBarFill != null)
                staminaBarFill.color = isDanger ? dangerStaminaColor : normalStaminaColor;
        }

        public void UpdateDragonBallCount(int count)
        {
            if (dragonBallCountText != null)
                dragonBallCountText.text = $"{count}";
        }

        public void ActivateNotificationPanel() => notificationPanel.SetActive(true);

        public void ShowGameOverPanel() => gameOverPanel.SetActive(true);

        private void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        private void ExitToMainMenu() => SceneManager.LoadScene(0);
    }
}