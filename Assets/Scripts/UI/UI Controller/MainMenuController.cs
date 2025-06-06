using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DragonBall.Sound.SoundUtilities;
using DragonBall.Sound.SoundData;

namespace DragonBall.UI.UIController
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        [Header("Scene Names")]
        [SerializeField] private string instructionsSceneName = "Instructions";

        private void Awake()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnStartButtonClicked()
        {
            SoundManager.Instance.PlaySoundEffect(SoundType.StartUIButton);
            SceneManager.LoadScene(instructionsSceneName);
        }

        private void OnQuitButtonClicked()
        {
            SoundManager.Instance.PlaySoundEffect(SoundType.QuitUIButton);
            QuitGame();
        }

        private void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                PlayerPrefs.DeleteAll();
            #else
                Application.Quit();
            #endif
        }
    }
}