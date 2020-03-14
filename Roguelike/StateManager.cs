namespace Roguelike
{
    /// <summary>
    /// Class for convenient change between game states.
    /// Automatically calls Invoke and Close methods.
    /// </summary>
    public class StateManager
    {
        public IGameState CurrentState { get; private set; }

        private static StateManager instance;

        private StateManager()
        {
        }

        public static StateManager GetInstance()
        {
            return instance ?? (instance = new StateManager());
        }

        public void ChangeState(IGameState newGameState)
        {
            CurrentState?.CloseState();
            CurrentState = newGameState;
            CurrentState?.InvokeState();
        }
    }
}