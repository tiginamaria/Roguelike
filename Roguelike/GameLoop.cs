namespace Roguelike
{
    /// <summary>
    /// Main loop for the game.
    /// </summary>
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
        
        /// <summary>
        /// Starts an infinite loop and updates the current game state.
        /// </summary>
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