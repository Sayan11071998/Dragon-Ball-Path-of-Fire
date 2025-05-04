using UnityEngine;
using UnityEngine.UI;
using DragonBall.Core;
using TMPro;

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
                UpdateDragonBallCount(playerModel.DragonBallCount);
            }
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
    }
}