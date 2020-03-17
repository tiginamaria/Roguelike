namespace Roguelike
{
    /// <summary>
    /// Represents a single play mode.
    /// </summary>
    public class PlayGameState : IGameState
    {
        private ILevelFactory levelFactory;
        private Level currentLevel;
        private ConsolePlayView playView = new ConsolePlayView();
        private MoveConsoleController moveConsoleController;
        private ExitGameController exitGameController;

        public PlayGameState(ILevelFactory levelFactory)
        {
            this.levelFactory = levelFactory;
        }
        
        /// <summary>
        /// Updates the controllers.
        /// </summary>
        public void Update()
        {
            exitGameController.Update();
            moveConsoleController.Update();
        }

        public void InvokeState()
        {
            currentLevel = levelFactory.CreateLevel();
            
            var moveInteractor = new MoveInteractor(currentLevel, playView);
            moveConsoleController = new MoveConsoleController(moveInteractor);
            
            exitGameController = new ExitGameController();
            
            playView.Draw(currentLevel);
        }
    }
}