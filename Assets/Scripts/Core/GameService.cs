using UnityEngine;
using DragonBall.Player;
using DragonBall.Utilities;


namespace DragonBall.Core
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        public PlayerService playerService { get; private set; }

        [Header("Player")]
        [SerializeField] private PlayerView playerView;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;

        protected override void Awake()
        {
            base.Awake();

            playerService = new PlayerService(playerView, playerScriptableObject);
        }

        private void Update()
        {
            playerService.Update();
        }
    }
}