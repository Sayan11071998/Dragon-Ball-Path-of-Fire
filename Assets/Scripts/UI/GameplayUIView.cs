using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DragonBall.Core;

namespace DragonBall.UI
{
    public class GameplayUIView : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Image healthBarFill; // Reference to the fill image of the health bar
        [SerializeField] private Color normalHealthColor = Color.green;
        [SerializeField] private Color dangerHealthColor = Color.red;

        private void Start()
        {
            if (GameService.Instance != null && GameService.Instance.playerService != null)
            {
                healthSlider.maxValue = GameService.Instance.playerService.PlayerController.PlayerModel.MaxHealth;
                healthSlider.value = GameService.Instance.playerService.PlayerController.PlayerModel.CurrentHealth;
            }
            else
            {
                Debug.LogError("GameService or playerService not initialized!");
            }
        }

        public void UpdateHealthBar(float healthPercentage)
        {
            healthSlider.value = healthPercentage * healthSlider.maxValue;
        }

        public void SetHealthBarDangerState(bool isDanger)
        {
            if (healthBarFill != null)
            {
                healthBarFill.color = isDanger ? dangerHealthColor : normalHealthColor;
            }
            else
            {
                Debug.LogWarning("Health bar fill reference is missing!");
            }
        }
    }
}