using UnityEngine;
using DragonBall.Core;
using DragonBall.Player.PlayerMVC;

namespace DragonBall.Pickup
{
    public class DragonBallPickup : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerView playerView = collider.GetComponent<PlayerView>();

                if (playerView != null)
                {
                    PlayerController playerController = GameService.Instance.playerService?.PlayerController;

                    if (playerController != null)
                    {
                        playerController.CollectDragonBall();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}