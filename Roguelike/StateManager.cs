using Roguelike.Initialization;

namespace Roguelike
{
    /// <summary>
    /// A class for convenient change between game states.
    /// Automatically calls Invoke method.
    /// </summary>
    public class StateManager
    {
        private IGameState currentState;

        private static StateManager instance;

        private StateManager()
        {
        }

        public static StateManager GetInstance() => instance ??= new StateManager();

        /// <summary>
        /// Switches to the given state and calls its invoke() method.
        /// </summary>
        public void ChangeState(IGameState newGameState)
        {
            currentState = newGameState;
            currentState?.InvokeState();
        }
    }
}
