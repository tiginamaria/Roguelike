namespace Roguelike
{
    public class PlayGameState : IGameState
    {
        //TODO: assign somewhere
        private LevelFactory levelFactory;
        private Level currentLevel;
        private PlayView playView;

        public void Update()
        {
            currentLevel.Update();
        }

        public void Draw()
        {
            playView.Draw(currentLevel);
        }

        public void InvokeState()
        {
            currentLevel = levelFactory.CreateLevel();
        }

        public void CloseState()
        {
            throw new System.NotImplementedException();
        }
    }
}