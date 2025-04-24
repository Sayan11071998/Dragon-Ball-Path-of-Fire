using UnityEngine;
using DragonBall.Player;
using DragonBall.Utilities;
using Unity.Cinemachine;


namespace DragonBall.Core
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        public PlayerService playerService { get; private set; }

        [Header("Player")]
        [SerializeField] private PlayerView playerView;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;

        [Header("Camera")]
        [SerializeField] CinemachineCamera cinemachineCamera;

        protected override void Awake()
        {
            base.Awake();

            playerService = new PlayerService(playerView, playerScriptableObject);
            cinemachineCamera.Follow = playerService.PlayerPrefab.transform;
        }

        private void Update()
        {
            playerService.Update();
        }
    }
}