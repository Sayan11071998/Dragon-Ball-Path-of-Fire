using System.Collections.Generic;
using UnityEngine;
using DragonBall.Player;
using DragonBall.Utilities;
using Unity.Cinemachine;
using DragonBall.VFX;
using DragonBall.Bullet;
using DragonBall.Enemy;
using DragonBall.UI;
using DragonBall.Camera;

namespace DragonBall.Core
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        public PlayerService playerService { get; private set; }
        public VFXService vFXService { get; private set; }
        public BulletService bulletService { get; private set; }
        public EnemyService enemyService { get; private set; }
        public UIService uiService { get; private set; }
        public CameraService cameraService { get; private set; }

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
        [SerializeField] private BulletView enemyNormalBulletPrefab;
        [SerializeField] private BulletScriptableObject enemyNormalBulletSO;
        [SerializeField] private GuidedBulletView enemyGuidedBullletPrefab;
        [SerializeField] private GuidedBulletScriptableObject enemyGuidedBulletSO;

        [Header("Enemy")]
        [SerializeField] private List<EnemyConfig> enemyConfigs;

        [Header("UI")]
        [SerializeField] private GameplayUIView gameplayUIViewPrefab;

        [Header("Camera Configuration")]
        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private CameraController cameraControllerPrefab;

        protected override void Awake()
        {
            base.Awake();
            InitializeServices();
        }

        private void InitializeServices()
        {
            cameraService = new CameraService(mainCamera, cameraControllerPrefab);
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
                { BulletType.EnemyRegularPowerBall, (enemyNormalBulletPrefab, enemyNormalBulletSO) },
                { BulletType.EnemyGuidedPowerBall, (enemyGuidedBullletPrefab, enemyGuidedBulletSO) }
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
    }

    [System.Serializable]
    public class EnemyConfig
    {
        public EnemyType type;
        public BaseEnemyView prefab;
        public EnemyScriptableObject so;
    }
}