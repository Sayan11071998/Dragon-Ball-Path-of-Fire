using UnityEngine;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerData;
using DragonBall.GameStrings;
using UnityEngine.SceneManagement;

namespace DragonBall.Player.PlayerUtilities
{
    public class PlayerService
    {
        private PlayerModel playerModel;
        private PlayerController playerController;
        private PlayerView playerPrefab;

        public PlayerView PlayerPrefab => playerPrefab;
        public PlayerController PlayerController => playerController;

        public PlayerService(PlayerView _playerPrefab, PlayerScriptableObject _config)
        {
            playerPrefab = Object.Instantiate(_playerPrefab);
            playerPrefab.name = GameString.PlayerPrefabName;

            playerModel = new PlayerModel(
                _config.PlayerHealth,
                _config.PlayerStamina,
                _config.StaminaRegenRate,
                _config.MoveSpeed,
                _config.JumpSpeed,
                _config.JumpHorizontalDampening,
                _config.FlySpeed,
                _config.VanishRange,
                _config.DodgeSpeed,
                _config.DodgeDuration,
                _config.DodgeCooldown,
                _config.KickAttackPower,
                _config.KickAttackRange,
                _config.KickAttackCooldown,
                _config.FireCooldown,
                _config.DragonBallsRequiredForSuperSaiyan,
                _config.SuperSaiyanHealthMultiplier,
                _config.SuperSaiyanStaminaMultiplier,
                _config.SuperSaiyanSpeedMultiplier,
                _config.SuperSaiyanPowerMultiplier,
                _config.KamehamehaStaminaCost
            );

            PlayerState initialState = DetermineInitialStateByScene();
            playerController = new PlayerController(playerModel, playerPrefab, initialState);
        }

        private PlayerState DetermineInitialStateByScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (currentSceneName.Contains(GameString.SceneFinalBoss))
                return PlayerState.SUPER_SAIYAN;

            return PlayerState.NORMAL;
        }

        public void Update() => playerController.Update();
    }
}