using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DragonBall.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        [Header("Scene Names")]
        [SerializeField] private string gameplaySceneName = "Gameplay";

        private void Awake()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnStartButtonClicked() => SceneManager.LoadScene(gameplaySceneName);

        private void OnQuitButtonClicked() => QuitGame();

        private void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}