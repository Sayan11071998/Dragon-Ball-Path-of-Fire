using UnityEngine;
using UnityEngine.UI;
using DragonBall.Core;

namespace DragonBall.UI
{
    public class GameplayUIView : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Image healthBarFill;
        [SerializeField] private Color normalHealthColor = Color.green;
        [SerializeField] private Color dangerHealthColor = Color.red;

        private void Start()
        {
            if (GameService.Instance != null && GameService.Instance.playerService != null)
            {
                healthSlider.maxValue = GameService.Instance.playerService.PlayerController.PlayerModel.MaxHealth;
                healthSlider.value = GameService.Instance.playerService.PlayerController.PlayerModel.CurrentHealth;
            }
        }

        public void UpdateHealthBar(float healthPercentage) => healthSlider.value = healthPercentage * healthSlider.maxValue;

        public void SetHealthBarDangerState(bool isDanger)
        {
            if (healthBarFill != null)
                healthBarFill.color = isDanger ? dangerHealthColor : normalHealthColor;
        }
    }
}