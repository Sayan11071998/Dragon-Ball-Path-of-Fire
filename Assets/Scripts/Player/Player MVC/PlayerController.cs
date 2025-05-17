public class PlayerController
    {
        private PlayerModel playerModel;
        private PlayerView playerView;
        private PlayerStateManager stateManager;
        
        public PlayerModel PlayerModel => playerModel;
        public PlayerView PlayerView => playerView;
        public PlayerStateManager StateManager => stateManager;
        
        private bool isInputEnabled = true;
        
        public PlayerController(PlayerModel _playerModel, PlayerView _playerView)
        {
            playerModel = _playerModel;
            playerView = _playerView;
            
            playerView.SetPlayerController(this);
            
            stateManager = new PlayerStateManager(this);
        }
        
        public void Update()
        {
            if (playerModel.IsDead) return;
            
            HandleGroundCheck();
            
            stateManager.Update();
        }
        
        private void HandleGroundCheck()
        {
            playerModel.IsGrounded = playerView.IsTouchingGround();
            
            if (playerModel.IsGrounded)
                playerModel.JumpCount = 0;
        }
        
        public void CollectDragonBall()
        {
            playerModel.IncrementDragonBallCount();
            SoundManager.Instance.PlaySoundEffect(SoundType.DragonBallCollect);
        }
        
        public void TakeDamage(float damage)
        {
            if (playerModel.IsDead) return;
            
            playerModel.TakeDamage(damage);
            GameService.Instance.cameraShakeService.ShakeCamera(0.1f, 0.5f);
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuTakeDamage);
        }
        
        public bool DisablePlayerController()
        {
            isInputEnabled = false;
            playerView.DisableInput();
            
            var velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = 0;
            playerView.Rigidbody.linearVelocity = velocity;
            
            return isInputEnabled;
        }
        
        public bool EnablePlayerController()
        {
            var velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = 0;
            playerView.Rigidbody.linearVelocity = velocity;
            
            isInputEnabled = true;
            playerView.EnableInput();
            return isInputEnabled;
        }
    }