namespace Roguelike
{
    public class PlayGameState : IGameState
    {
        private ILevelFactory levelFactory;
        private Level currentLevel;
        private PlayView playView = new PlayView();
        private MoveController moveController;

        public PlayGameState(ILevelFactory levelFactory)
        {
            this.levelFactory = levelFactory;
        }
        
        public void Update()
        {
            moveController.Update();
            currentLevel.Update();
        }

        public void Draw()
        {
            playView.Draw(currentLevel);
        }

        public void InvokeState()
        {
            currentLevel = levelFactory.CreateLevel();
            moveController = new MoveController(currentLevel.Player);
        }

        public void CloseState()
        {
            throw new System.NotImplementedException();
        }
    }
}