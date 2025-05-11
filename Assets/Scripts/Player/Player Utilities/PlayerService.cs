using UnityEngine;
using DragonBall.GameStrings;
using System.Collections;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerData;
using DragonBall.Utilities;

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

            playerController = new PlayerController(playerModel, playerPrefab);
            ApplyPersistedState();
        }

        public void Update() => playerController.Update();

        private void ApplyPersistedState()
        {
            if (PlayerPrefs.HasKey(GameStateUtility.DRAGON_BALL_COUNT_KEY))
            {
                int dragonBallCount = PlayerPrefs.GetInt(GameStateUtility.DRAGON_BALL_COUNT_KEY, 0);

                for (int i = 0; i < dragonBallCount; i++)
                    playerModel.IncrementDragonBallCount();

                bool isSuperSaiyan = PlayerPrefs.GetInt(GameStateUtility.SUPER_SAIYAN_KEY, 0) == 1;

                if (isSuperSaiyan)
                    playerPrefab.StartCoroutine(ApplySuperSaiyanStateWithDelay());
            }
        }

        private IEnumerator ApplySuperSaiyanStateWithDelay()
        {
            yield return null;

            if (playerController != null && !playerPrefab.IsSuperSaiyan)
            {
                var stateMachine = playerController.GetPlayerStateMachine();
                if (stateMachine != null)
                    stateMachine.ChangeState(PlayerState.SUPER_SAIYAN);
            }
        }
    }
}