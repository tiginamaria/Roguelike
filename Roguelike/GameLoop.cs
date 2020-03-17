namespace Roguelike
{
    public class GameLoop
    {
        private StateManager stateManager;
        private bool stopped;
        
        public GameLoop()
        {
            stateManager = StateManager.GetInstance();
            stateManager.ExitEvent += 
                (sender, args) =>
                {
                    stopped = true;
                };
        }
        
        public void Run(IGameState startState)
        {
            stopped = false;
            stateManager.ChangeState(startState);
            
            while (!stopped)
            {
                Keyboard.Update();
                stateManager.CurrentState.Update();
            }
        }
    }
}