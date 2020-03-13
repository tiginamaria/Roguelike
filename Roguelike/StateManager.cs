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

        public void ChangeState(StateType newStateType)
        {
            CurrentState?.CloseState();

            switch (newStateType)
            {
//                case StateType.Start:
//                    CurrentState = new StartGameState();
//                    break;
                case StateType.Play:
                    CurrentState = new PlayGameState();
                    break;
//                case StateType.ResultWin:
//                    CurrentState = new ResultGameState(true);
//                    break;
//                case StateType.ResultLose:
//                    CurrentState = new ResultGameState(false);
//                    break;
            }

            CurrentState?.InvokeState();
        }
    }
}