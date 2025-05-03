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
        [SerializeField] private Button actionButton;
        [SerializeField] private TextMeshProUGUI buttonText;

        [Header("Scene Management")]
        [SerializeField] private string gameplaySceneName = "Gameplay";
        [SerializeField] private bool enableButtonAfterTyping = true;

        [Header("Text Animation Settings")]
        [SerializeField] private float typingSpeed = 0.05f;
        [SerializeField] private float delayBetweenLines = 0.5f;
        [SerializeField] private AudioSource typingSoundEffect;

        [Header("Movement Instructions")]
        [SerializeField] private string movePanelTitle = "MOVE ACTIONS";
        [SerializeField] private string moreButtonText = "MORE";
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
        [SerializeField] private string continueButtonText = "CONTINUE";
        [TextArea(3, 5)]
        [SerializeField]
        private List<string> attackInstructionLines = new List<string>
        {
            "Press 'V' to Instant Transport",
            "Press 'F' to Dodge",
            "Press 'RMB' to Kick",
            "Press 'LMB' to Fire",
            "Hold 'K' to Charge Kamehameha"
        };

        private Coroutine typingCoroutine;
        private bool isTyping = false;
        private bool skipTyping = false;
        private bool isMovementMode = true;

        private void Awake()
        {
            if (actionButton != null)
            {
                actionButton.onClick.AddListener(OnActionButtonClicked);
                if (enableButtonAfterTyping)
                    actionButton.gameObject.SetActive(false);
            }
            SetPanelMode(true);
        }

        private void Start() => StartTypingInstructions();

        private void SetPanelMode(bool movementMode)
        {
            isMovementMode = movementMode;

            if (headerText != null)
                headerText.text = movementMode ? movePanelTitle : attackPanelTitle;

            if (buttonText != null)
                buttonText.text = movementMode ? moreButtonText : continueButtonText;

            if (bodyText != null)
                bodyText.text = "";
        }

        public void StartTypingInstructions()
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            List<string> currentInstructions = isMovementMode ? moveInstructionLines : attackInstructionLines;
            typingCoroutine = StartCoroutine(TypeInstructionsSequentially(currentInstructions));
        }

        public void SkipTypingAnimation()
        {
            if (isTyping)
                skipTyping = true;
        }

        private IEnumerator TypeInstructionsSequentially(List<string> instructionLines)
        {
            isTyping = true;

            if (enableButtonAfterTyping && actionButton != null)
                actionButton.gameObject.SetActive(false);

            if (bodyText != null)
            {
                bodyText.text = "";

                for (int i = 0; i < instructionLines.Count; i++)
                {
                    yield return TypeTextLine(instructionLines[i]);

                    if (i < instructionLines.Count - 1)
                    {
                        bodyText.text += "\n";
                        yield return new WaitForSeconds(delayBetweenLines);
                    }
                }
            }

            isTyping = false;

            if (enableButtonAfterTyping && actionButton != null)
                actionButton.gameObject.SetActive(true);
        }

        private IEnumerator TypeTextLine(string line)
        {
            skipTyping = false;
            string currentText = bodyText.text;

            for (int i = 0; i < line.Length; i++)
            {
                if (skipTyping)
                {
                    bodyText.text = currentText + line;

                    if (typingSoundEffect != null)
                        typingSoundEffect.Stop();
                    break;
                }

                bodyText.text = currentText + line.Substring(0, i + 1);

                if (typingSoundEffect != null && !typingSoundEffect.isPlaying)
                    typingSoundEffect.Play();

                yield return new WaitForSeconds(typingSpeed);
            }

            if (typingSoundEffect != null)
                typingSoundEffect.Stop();
        }

        private void OnActionButtonClicked()
        {
            if (isTyping)
            {
                SkipTypingAnimation();
                return;
            }

            if (isMovementMode)
            {
                SetPanelMode(false);
                StartTypingInstructions();
            }
            else
            {
                SceneManager.LoadScene(gameplaySceneName);
            }
        }

        private void Update()
        {
            if (isTyping && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
                SkipTypingAnimation();
        }
    }
}