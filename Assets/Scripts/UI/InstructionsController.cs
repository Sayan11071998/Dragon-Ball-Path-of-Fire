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

        private Image panelImage;
        private Coroutine typingCoroutine;

        private bool isTyping = false;
        private bool skipTyping = false;

        private void Awake()
        {
            panelImage = instructionPanel.GetComponent<Image>();

            if (bodyText != null)
                bodyText.text = "";

            if (headerText != null)
                headerText.text = panelTitle;

            if (continueButton != null)
            {
                continueButton.onClick.AddListener(OnContinueButtonClicked);
                if (enableContinueAfterTyping)
                    continueButton.gameObject.SetActive(false);
            }
        }

        private void Start() => StartInstructionSequence();

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

            if (enableContinueAfterTyping && continueButton != null)
                continueButton.gameObject.SetActive(false);

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
            if (enableContinueAfterTyping && continueButton != null)
                continueButton.gameObject.SetActive(true);
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

        private void OnContinueButtonClicked()
        {
            if (isTyping)
            {
                SkipTypingAnimation();
                return;
            }
            SceneManager.LoadScene(gameplaySceneName);
        }

        private void Update()
        {
            if (isTyping && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
                SkipTypingAnimation();
        }
    }
}