using System.Collections.Generic;
using UnityEngine;
using DragonBall.Player;
using DragonBall.Utilities;
using Unity.Cinemachine;
using DragonBall.VFX;
using DragonBall.Bullet;
using DragonBall.Enemy;
using DragonBall.UI;

namespace DragonBall.Core
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        public PlayerService playerService { get; private set; }
        public VFXService vFXService { get; private set; }
        public BulletService bulletService { get; private set; }
        public EnemyService enemyService { get; private set; }
        public UIService uiService { get; private set; }

        [Header("Player")]
        [SerializeField] private PlayerView playerView;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;

        [Header("VFX")]
        [SerializeField] private VFXView vFXPrefab;

        [Header("Bullet")]
        [SerializeField] private BulletView playerNormalBulletPrefab;
        [SerializeField] private BulletScriptableObject playerNormalBulletSO;
        [SerializeField] private BulletView playerSuperSaiyanBulletPrefab;
        [SerializeField] private BulletScriptableObject playerSuperSaiyanBulletSO;
        [SerializeField] private BulletView playerKamehamehaPrefab;
        [SerializeField] private BulletScriptableObject playerKamehamehaSO;
        [SerializeField] private BulletView enemyBulletPrefab;
        [SerializeField] private BulletScriptableObject enemyBulletSO;

        [Header("Enemy")]
        [SerializeField] private List<EnemyConfig> enemyConfigs;

        [Header("UI")]
        [SerializeField] private GameplayUIView gameplayUIViewPrefab;

        [Header("Cinemachine Virtual Camera")]
        [SerializeField] private CinemachineStateDrivenCamera cinemachineStateDrivenCamera;
        [SerializeField] private CinemachineCamera idleCamera;
        [SerializeField] private CinemachineCamera runCamera;
        [SerializeField] private CinemachineCamera jumpCamera;

        protected override void Awake()
        {
            base.Awake();
            InitializeServices();
            InitializeVirtualCamera();
        }

        private void InitializeServices()
        {
            playerService = new PlayerService(playerView, playerScriptableObject);
            vFXService = new VFXService(vFXPrefab);

            InitializeEnemyService();
            InitializeBulletService();
            InitializeUIService();
        }

        private void InitializeEnemyService()
        {
            var enemyConfigsDict = new Dictionary<EnemyType, (BaseEnemyView, EnemyScriptableObject)>();
            foreach (var config in enemyConfigs)
                enemyConfigsDict[config.type] = (config.prefab, config.so);
            enemyService = new EnemyService(enemyConfigsDict);
        }

        private void InitializeBulletService()
        {
            var bulletConfigs = new Dictionary<BulletType, (BulletView, BulletScriptableObject)>
            {
                { BulletType.PlayerNormalPowerBall, (playerNormalBulletPrefab, playerNormalBulletSO) },
                { BulletType.PlayerSuperSaiyanPowerBall, (playerSuperSaiyanBulletPrefab, playerSuperSaiyanBulletSO) },
                { BulletType.PlayerKamehamehaPowerBall, (playerKamehamehaPrefab, playerKamehamehaSO) },
                { BulletType.EnemyRegularPowerBall, (enemyBulletPrefab, enemyBulletSO) }
            };
            bulletService = new BulletService(bulletConfigs);
        }

        private void InitializeUIService()
        {
            if (gameplayUIViewPrefab != null)
                uiService = new UIService(gameplayUIViewPrefab, playerService.PlayerController.PlayerModel);
            else
                Debug.LogError("GameplayUIView prefab reference is missing in GameService!");
        }

        private void Update()
        {
            playerService.Update();
            uiService.Update();
        }

        private void InitializeVirtualCamera()
        {
            idleCamera.Follow = playerService.PlayerPrefab.transform;
            runCamera.Follow = playerService.PlayerPrefab.transform;
            jumpCamera.Follow = playerService.PlayerPrefab.transform;
            cinemachineStateDrivenCamera.AnimatedTarget = playerService.PlayerPrefab.Animator;
        }
    }

    [System.Serializable]
    public class EnemyConfig
    {
        public EnemyType type;
        public BaseEnemyView prefab;
        public EnemyScriptableObject so;
    }
}