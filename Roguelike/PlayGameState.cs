namespace Roguelike
{
    public class PlayGameState : IGameState
    {
        private ILevelFactory levelFactory;
        private Level currentLevel;
        private ConsolePlayView playView = new ConsolePlayView();
        private MoveConsoleController moveConsoleController;

        public PlayGameState(ILevelFactory levelFactory)
        {
            this.levelFactory = levelFactory;
        }
        
        public void Update()
        {
            moveConsoleController.Update();
        }

        public void InvokeState()
        {
            currentLevel = levelFactory.CreateLevel();
            var moveInteractor = new MoveInteractor(currentLevel, playView);
            moveConsoleController = new MoveConsoleController(moveInteractor);
            playView.Draw(currentLevel);
        }
    }
}