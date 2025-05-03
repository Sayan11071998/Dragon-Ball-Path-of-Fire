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
        [Header("Movement Panel References")]
        [SerializeField] private GameObject movePanel;
        [SerializeField] private TextMeshProUGUI moveHeaderText;
        [SerializeField] private TextMeshProUGUI moveBodyText;
        [SerializeField] private Button moreButton;

        [Header("Attack Panel References")]
        [SerializeField] private GameObject attackPanel;
        [SerializeField] private TextMeshProUGUI attackHeaderText;
        [SerializeField] private TextMeshProUGUI attackBodyText;
        [SerializeField] private Button continueButton;

        [Header("Scene Management")]
        [SerializeField] private string gameplaySceneName = "Gameplay";
        [SerializeField] private bool enableButtonsAfterTyping = true;

        [Header("Text Animation Settings")]
        [SerializeField] private float typingSpeed = 0.05f;
        [SerializeField] private float delayBetweenLines = 0.5f;
        [SerializeField] private AudioSource typingSoundEffect;

        [Header("Movement Instructions")]
        [SerializeField] private string movePanelTitle = "MOVE ACTIONS";
        [TextArea(3, 5)]
        [SerializeField]
        private List<string> moveInstructionLines = new List<string>
        {
            "Press 'D' to Move Right",
            "Press 'A' to Move Left",
            "Press 'Spacebar' to Jump",
            "Press 'Spacebar' twice to Double Jump"
        };

        [Header("Attack Instructions")]
        [SerializeField] private string attackPanelTitle = "ATTACK ACTIONS";
        [TextArea(3, 5)]
        [SerializeField]
        private List<string> attackInstructionLines = new List<string>
        {
            "Press 'J' to Punch",
            "Press 'K' to Kick",
            "Press 'L' to Shoot Energy Ball",
            "Hold 'L' to Charge Kamehameha"
        };

        private Coroutine typingCoroutine;
        private bool isTyping = false;
        private bool skipTyping = false;
        private bool isMovePanelActive = true;

        private void Awake()
        {
            // Setup Move Panel
            if (moveHeaderText != null)
                moveHeaderText.text = movePanelTitle;
            if (moveBodyText != null)
                moveBodyText.text = "";
            if (moreButton != null)
            {
                moreButton.onClick.AddListener(OnMoreButtonClicked);
                if (enableButtonsAfterTyping)
                    moreButton.gameObject.SetActive(false);
            }

            // Setup Attack Panel
            if (attackHeaderText != null)
                attackHeaderText.text = attackPanelTitle;
            if (attackBodyText != null)
                attackBodyText.text = "";
            if (continueButton != null)
            {
                continueButton.onClick.AddListener(OnContinueButtonClicked);
                if (enableButtonsAfterTyping)
                    continueButton.gameObject.SetActive(false);
            }

            // Set initial panel states
            SetPanelStates(true);
        }

        private void Start()
        {
            // Start typing animation for the movement panel
            StartMovementInstructionSequence();
        }

        private void SetPanelStates(bool moveActive)
        {
            isMovePanelActive = moveActive;

            if (movePanel != null)
                movePanel.SetActive(moveActive);

            if (attackPanel != null)
                attackPanel.SetActive(!moveActive);
        }

        public void StartMovementInstructionSequence()
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeInstructionsSequentially(
                moveBodyText,
                moveInstructionLines,
                moreButton));
        }

        public void StartAttackInstructionSequence()
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeInstructionsSequentially(
                attackBodyText,
                attackInstructionLines,
                continueButton));
        }

        public void SkipTypingAnimation()
        {
            if (isTyping)
                skipTyping = true;
        }

        private IEnumerator TypeInstructionsSequentially(
            TextMeshProUGUI textComponent,
            List<string> instructionLines,
            Button buttonToEnable)
        {
            isTyping = true;

            if (enableButtonsAfterTyping && buttonToEnable != null)
                buttonToEnable.gameObject.SetActive(false);

            if (textComponent != null)
            {
                textComponent.text = "";

                for (int i = 0; i < instructionLines.Count; i++)
                {
                    yield return TypeTextLine(textComponent, instructionLines[i]);

                    if (i < instructionLines.Count - 1)
                    {
                        textComponent.text += "\n";
                        yield return new WaitForSeconds(delayBetweenLines);
                    }
                }
            }

            isTyping = false;

            if (enableButtonsAfterTyping && buttonToEnable != null)
                buttonToEnable.gameObject.SetActive(true);
        }

        private IEnumerator TypeTextLine(TextMeshProUGUI textComponent, string line)
        {
            skipTyping = false;
            string currentText = textComponent.text;

            for (int i = 0; i < line.Length; i++)
            {
                if (skipTyping)
                {
                    textComponent.text = currentText + line;

                    if (typingSoundEffect != null)
                        typingSoundEffect.Stop();
                    break;
                }

                textComponent.text = currentText + line.Substring(0, i + 1);

                if (typingSoundEffect != null && !typingSoundEffect.isPlaying)
                    typingSoundEffect.Play();

                yield return new WaitForSeconds(typingSpeed);
            }

            if (typingSoundEffect != null)
                typingSoundEffect.Stop();
        }

        private void OnMoreButtonClicked()
        {
            if (isTyping)
            {
                SkipTypingAnimation();
                return;
            }

            // Switch to attack panel
            SetPanelStates(false);
            StartAttackInstructionSequence();
        }

        private void OnContinueButtonClicked()
        {
            if (isTyping)
            {
                SkipTypingAnimation();
                return;
            }

            // Load gameplay scene
            SceneManager.LoadScene(gameplaySceneName);
        }

        private void Update()
        {
            // Skip typing animation with any key/mouse click
            if (isTyping && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
                SkipTypingAnimation();
        }
    }
}