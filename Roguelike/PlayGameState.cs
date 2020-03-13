namespace Roguelike
{
    public class PlayGameState : IGameState
    {
        //TODO: assign somewhere
        private LevelFactory levelFactory;
        private Level currentLevel;

        public void Update()
        {
            currentLevel.Update();
        }

        public void Draw()
        {
            throw new System.NotImplementedException();
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