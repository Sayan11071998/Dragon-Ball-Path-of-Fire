using UnityEngine;
using DragonBall.Player.PlayerMVC;

namespace DragonBall.Level
{
    public class FreeFallTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerView playerView = collision.GetComponent<PlayerView>();
                if (playerView != null)
                    playerView.TriggerFreeFallDeath();
            }
        }
    }
}