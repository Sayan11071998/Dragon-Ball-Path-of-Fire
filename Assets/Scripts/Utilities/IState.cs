namespace DragonBall.Utilities
{
    public interface IState
    {
        public void OnStateEnter();
        public void Update();
        public void OnStateExit();
    }
}