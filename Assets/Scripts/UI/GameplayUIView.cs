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
        [SerializeField] private Image healthBarFill;
        [SerializeField] private TextMeshProUGUI dragonBallCountText;
        [SerializeField] private Color normalHealthColor = Color.green;
        [SerializeField] private Color dangerHealthColor = Color.red;

        [Header("Notification Panel")]
        [SerializeField] public GameObject notificationPanel;
        [SerializeField] public Button notificationButton;

        private void Start()
        {
            if (GameService.Instance?.playerService != null)
            {
                var model = GameService.Instance.playerService.PlayerController.PlayerModel;
                healthSlider.maxValue = model.MaxHealth;
                healthSlider.value = model.CurrentHealth;
                notificationPanel.SetActive(false);

                UpdateDragonBallCount(model.DragonBallCount);
            }
        }

        public void UpdateHealthBar(float healthPercentage) => healthSlider.value = healthPercentage * healthSlider.maxValue;

        public void SetHealthBarDangerState(bool isDanger)
        {
            if (healthBarFill != null)
                healthBarFill.color = isDanger ? dangerHealthColor : normalHealthColor;
        }

        public void UpdateDragonBallCount(int count)
        {
            if (dragonBallCountText != null)
                dragonBallCountText.text = $"{count}";
        }

        public void ActivateNotificationPanel() => notificationPanel.SetActive(true);
    }
}