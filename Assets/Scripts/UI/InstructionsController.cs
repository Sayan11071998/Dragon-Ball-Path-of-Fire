using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DragonBall.UI
{
    public class InstructionsController : MonoBehaviour
    {
        [Header("Panel References")]
        [SerializeField] private GameObject instructionPanel;
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private TextMeshProUGUI bodyText;
        [SerializeField] private Button continueButton;

        [Header("Scene Management")]
        [SerializeField] private string gameplaySceneName = "Gameplay";
        [SerializeField] private bool enableContinueAfterTyping = true;

        [Header("Text Animation Settings")]
        [SerializeField] private float typingSpeed = 0.05f;
        [SerializeField] private float delayBetweenLines = 0.5f;
        [SerializeField] private AudioSource typingSoundEffect;

        [Header("Instruction Text")]
        [SerializeField] private string panelTitle = "MOVE ACTIONS";
        [TextArea(3, 5)]
        [SerializeField]
        private List<string> instructionLines = new List<string>
        {
            "Press 'D' to Move Right",
            "Press 'A' to Move Left",
            "Press 'Spacebar' to Jump",
            "Press 'Spacebar' twice to Double Jump"
        };

        [Header("Visual Settings")]
        [SerializeField] private Color headerColor = new Color(0.6f, 0.4f, 0.0f); // Dark gold
        [SerializeField] private Color textColor = new Color(0.5f, 0.33f, 0.0f); // Darker gold for readability
        [SerializeField] private Color panelColor = new Color(1.0f, 0.9f, 0.2f, 0.9f); // Slightly transparent yellow
        [SerializeField] private Color continueButtonNormalColor = new Color(0.9f, 0.6f, 0.0f); // Orange
        [SerializeField] private Color continueButtonHighlightColor = new Color(1.0f, 0.7f, 0.0f); // Lighter orange

        private Image panelImage;
        private Coroutine typingCoroutine;
        private bool isTyping = false;
        private bool skipTyping = false;

        private void Awake()
        {
            // Get panel image component if not already assigned
            if (instructionPanel != null && panelImage == null)
                panelImage = instructionPanel.GetComponent<Image>();

            // Apply colors
            ApplyVisualSettings();

            // Hide text initially
            if (bodyText != null)
                bodyText.text = "";

            // Set header text
            if (headerText != null)
                headerText.text = panelTitle;

            // Set up continue button
            if (continueButton != null)
            {
                continueButton.onClick.AddListener(OnContinueButtonClicked);

                // Initially disable continue button if needed
                if (enableContinueAfterTyping)
                    continueButton.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            // Start typing animation
            StartInstructionSequence();
        }

        public void StartInstructionSequence()
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeInstructionsSequentially());
        }

        public void SkipTypingAnimation()
        {
            if (isTyping)
                skipTyping = true;
        }

        private IEnumerator TypeInstructionsSequentially()
        {
            isTyping = true;

            // Disable continue button during typing if needed
            if (enableContinueAfterTyping && continueButton != null)
                continueButton.gameObject.SetActive(false);

            if (bodyText != null)
            {
                bodyText.text = "";

                // Type each instruction line sequentially
                for (int i = 0; i < instructionLines.Count; i++)
                {
                    yield return TypeTextLine(instructionLines[i]);

                    // Add a new line if not the last instruction
                    if (i < instructionLines.Count - 1)
                    {
                        bodyText.text += "\n";
                        yield return new WaitForSeconds(delayBetweenLines);
                    }
                }
            }

            isTyping = false;

            // Show continue button after typing is complete
            if (enableContinueAfterTyping && continueButton != null)
                continueButton.gameObject.SetActive(true);
        }

        private IEnumerator TypeTextLine(string line)
        {
            skipTyping = false;
            string currentText = bodyText.text;

            for (int i = 0; i < line.Length; i++)
            {
                // Check if we should skip the typing animation
                if (skipTyping)
                {
                    bodyText.text = currentText + line;
                    if (typingSoundEffect != null)
                        typingSoundEffect.Stop();
                    break;
                }

                bodyText.text = currentText + line.Substring(0, i + 1);

                // Play typing sound
                if (typingSoundEffect != null && !typingSoundEffect.isPlaying)
                    typingSoundEffect.Play();

                yield return new WaitForSeconds(typingSpeed);
            }

            if (typingSoundEffect != null)
                typingSoundEffect.Stop();
        }

        private void ApplyVisualSettings()
        {
            if (headerText != null)
                headerText.color = headerColor;

            if (bodyText != null)
                bodyText.color = textColor;

            if (panelImage != null)
                panelImage.color = panelColor;

            if (continueButton != null)
            {
                ColorBlock colors = continueButton.colors;
                colors.normalColor = continueButtonNormalColor;
                colors.highlightedColor = continueButtonHighlightColor;
                continueButton.colors = colors;
            }
        }

        private void OnContinueButtonClicked()
        {
            // Skip typing if still typing
            if (isTyping)
            {
                SkipTypingAnimation();
                return;
            }

            // Load gameplay scene
            SceneManager.LoadScene(gameplaySceneName);
        }

        // If player presses any key during instructions
        private void Update()
        {
            // Allow skipping the typing animation with any key/mouse click
            if (isTyping && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
            {
                SkipTypingAnimation();
            }
        }
    }
}