using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DragonBall.Core;
using DragonBall.Utilities;
using DragonBall.GameStrings;

namespace DragonBall.Level
{
    public class LevelCompleteTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(GameString.PlayerTag)) return;

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