using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DragonBall.Core;

namespace DragonBall.Level
{
    public class LevelCompleteTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var playerController = GameService.Instance.playerService.PlayerController;
            var playerModel = playerController.PlayerModel;
            var playerView = playerController.PlayerView;

            GameStateUtility.SavePlayerState(playerView.IsSuperSaiyan, playerModel.DragonBallCount);

            playerController.DisablePlayerController();
            StartCoroutine(LoadNextSceneAfterDelay(2f));
        }

        private IEnumerator LoadNextSceneAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}