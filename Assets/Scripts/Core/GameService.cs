using UnityEngine;
using DragonBall.Player;
using DragonBall.Utilities;
using Unity.Cinemachine;
using DragonBall.VFX;
using DragonBall.Bullet;
using System.Collections.Generic;
using DragonBall.Enemy;

namespace DragonBall.Core
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        public PlayerService playerService { get; private set; }
        public VFXService vFXService { get; private set; }
        public BulletService bulletService { get; private set; }
        public EnemyService enemyService { get; private set; }

        [Header("Player")]
        [SerializeField] private PlayerView playerView;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;

        [Header("VFX")]
        [SerializeField] private VFXView vFXPrefab;

        [Header("Bullet")]
        [SerializeField] private BulletView regularBulletPrefab;
        [SerializeField] private BulletScriptableObject regularBulletSO;
        [SerializeField] private BulletView kamehamehaPrefab;
        [SerializeField] private BulletScriptableObject kamehamehaSO;

        [Header("Enemy")]
        [SerializeField] private EnemyPool enemyPool;
        [SerializeField] private EnemyScriptableObject enemyScriptableObject;

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
            enemyService = new EnemyService(enemyPool, enemyScriptableObject);
            InitializeBulletService();
        }

        private void InitializeBulletService()
        {
            var bulletConfigs = new Dictionary<BulletType, (BulletView, BulletScriptableObject)>
            {
                { BulletType.Regular, (regularBulletPrefab, regularBulletSO) },
                { BulletType.Kamehameha, (kamehamehaPrefab, kamehamehaSO) }
            };
            bulletService = new BulletService(bulletConfigs);
        }

        private void Update()
        {
            playerService.Update();
        }

        private void InitializeVirtualCamera()
        {
            idleCamera.Follow = playerService.PlayerPrefab.transform;
            runCamera.Follow = playerService.PlayerPrefab.transform;
            jumpCamera.Follow = playerService.PlayerPrefab.transform;
            cinemachineStateDrivenCamera.AnimatedTarget = playerService.PlayerPrefab.Animator;
        }
    }
}