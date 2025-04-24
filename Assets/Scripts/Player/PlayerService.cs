using UnityEngine;

namespace DragonBall.Player
{
    public class PlayerService
    {
        private PlayerModel playerModel;
        private PlayerController playerController;
        private PlayerView playerPrefab;

        public PlayerView PlayerPrefab => playerPrefab;

        public PlayerService(PlayerView _playerPrefab, PlayerScriptableObject _config)
        {
            playerPrefab = Object.Instantiate(_playerPrefab);
            playerPrefab.name = "Songoku";

            playerModel = new PlayerModel
                (
                    _config.PlayerHealth,
                    _config.MoveSpeed,
                    _config.JumpSpeed,
                    _config.JumpHorizontalDampening,
                    _config.vanishRange
                );

            playerController = new PlayerController(playerModel, playerPrefab);
        }

        public void Update() => playerController.Update();
    }
}