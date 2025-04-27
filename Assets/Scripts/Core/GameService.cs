using UnityEngine;
using DragonBall.Player;
using DragonBall.Utilities;
using Unity.Cinemachine;
using DragonBall.VFX;
using DragonBall.Bullet;
using DragonBall.Enemy;
using System.Collections.Generic;

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
        [SerializeField] private EnemyView friezaPrefab;
        [SerializeField] private EnemyScriptableObject friezaSO;
        [SerializeField] private EnemyView cellPrefab;
        [SerializeField] private EnemyScriptableObject cellSO;
        [SerializeField] private EnemyView buuPrefab;
        [SerializeField] private EnemyScriptableObject buuSO;

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

            var bulletConfigs = new Dictionary<BulletType, (BulletView, BulletScriptableObject)>
            {
                { BulletType.Regular, (regularBulletPrefab, regularBulletSO) },
                { BulletType.Kamehameha, (kamehamehaPrefab, kamehamehaSO) }
            };
            bulletService = new BulletService(bulletConfigs);

            var enemyConfigs = new Dictionary<EnemyType, (EnemyView, EnemyScriptableObject)>
            {
                { EnemyType.FRIEZA, (friezaPrefab, friezaSO) },
                { EnemyType.CELL, (cellPrefab, cellSO) },
                { EnemyType.BUU, (buuPrefab, buuSO) }
            };
            enemyService = new EnemyService(enemyConfigs);
        }

        private void Update()
        {
            playerService.Update();
            enemyService.Update();
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