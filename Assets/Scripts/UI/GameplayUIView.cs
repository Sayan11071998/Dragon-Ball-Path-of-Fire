using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DragonBall.UI
{
    public class GameplayUIView : MonoBehaviour
    {
        [Header("Health UI")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private Image healthBarFill;
        [SerializeField] private TextMeshProUGUI healthText;

        [Header("Health Bar Colors")]
        [SerializeField] private Color normalHealthColor = Color.green;
        [SerializeField] private Color dangerHealthColor = Color.red;
        [SerializeField] private float healthBarSmoothTime = 0.2f;

        private float targetFillAmount;
        private float currentVelocity;

        private void Awake()
        {
            // Ensure references are set
            if (healthBar == null)
                Debug.LogError("Health Bar Slider reference is missing on GameplayUIView!");

            if (healthBarFill == null)
                Debug.LogError("Health Bar Fill reference is missing on GameplayUIView!");

            // Initialize UI
            healthBarFill.color = normalHealthColor;
        }

        private void Update()
        {
            // Smoothly animate health bar changes
            if (healthBar != null && healthBar.value != targetFillAmount)
            {
                healthBar.value = Mathf.SmoothDamp(healthBar.value, targetFillAmount, ref currentVelocity, healthBarSmoothTime);
            }
        }

        public void UpdateHealthBar(float fillPercentage)
        {
            // Update the target value for smooth animation
            targetFillAmount = Mathf.Clamp01(fillPercentage);

            // Update the numeric health text if available
            if (healthText != null)
            {
                healthText.text = $"{Mathf.CeilToInt(fillPercentage * 100)}%";
            }
        }

        public void SetHealthBarDangerState(bool isDanger)
        {
            if (healthBarFill != null)
            {
                healthBarFill.color = isDanger ? dangerHealthColor : normalHealthColor;
            }
        }
    }
}