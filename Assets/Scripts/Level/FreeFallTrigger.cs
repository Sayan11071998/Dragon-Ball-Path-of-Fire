using UnityEngine;
using DragonBall.Player.PlayerMVC;
using DragonBall.GameStrings;

namespace DragonBall.Level
{
    public class FreeFallTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(GameString.PlayerTag))
            {
                PlayerView playerView = collision.GetComponent<PlayerView>();
                if (playerView != null)
                    playerView.TriggerFreeFallDeath();
            }
        }
    }
}