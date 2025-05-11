using UnityEngine;
using System.Collections.Generic;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerData;
using DragonBall.Bullet.BulletUtilities.BulletUtilities;
using DragonBall.Bullet.ParentMVC;
using DragonBall.Bullet.BulletData;
using DragonBall.Bullet.GuidedBulletMVC;
using DragonBall.Enemy.EnemyUtilities;
using DragonBall.Enemy.EnemyData;
using DragonBall.Enemy.ParentMVC;
using DragonBall.GameCamera;
using DragonBall.Utilities;
using DragonBall.Sound.SoundUtilities;
using DragonBall.Sound.SoundData;
using DragonBall.UI.UIUtilities;
using DragonBall.UI.UIView;
using DragonBall.VFX;

namespace DragonBall.Core
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        public PlayerService playerService { get; private set; }
        public VFXService vFXService { get; private set; }
        public BulletService bulletService { get; private set; }
        public EnemyService enemyService { get; private set; }
        public UIService uiService { get; private set; }
        public CameraShakeService cameraShakeService { get; private set; }

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
        [SerializeField] private GuidedBulletView finalBossEnemyGuidedBullletPrefab;
        [SerializeField] private GuidedBulletScriptableObject finalBossEnemyGuidedBulletSO;

        [Header("Enemy")]
        [SerializeField] private List<EnemyConfig> enemyConfigs;

        [Header("UI")]
        [SerializeField] private GameplayUIView gameplayUIViewPrefab;

        protected override void Awake()
        {
            base.Awake();
            InitializeServices();

            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayBackgroundMusic(SoundType.BackgroundMusic);
        }

        private void InitializeServices()
        {
            playerService = new PlayerService(playerView, playerScriptableObject);
            vFXService = new VFXService(vFXPrefab);
            InitializeCameraShakeService();
            InitializeEnemyService();
            InitializeBulletService();
            InitializeUIService();
        }

        private void InitializeCameraShakeService()
        {
            Camera mainCamera = Camera.main;
            cameraShakeService = new CameraShakeService(mainCamera, this);
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
                { BulletType.EnemyGuidedPowerBall, (enemyGuidedBullletPrefab, enemyGuidedBulletSO) },
                { BulletType.FinalBossEnemyGuidedPowerBall, (finalBossEnemyGuidedBullletPrefab, finalBossEnemyGuidedBulletSO) }
            };
            bulletService = new BulletService(bulletConfigs);
        }

        private void InitializeUIService()
        {
            if (gameplayUIViewPrefab != null)
                uiService = new UIService(gameplayUIViewPrefab, playerService.PlayerController.PlayerModel);
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