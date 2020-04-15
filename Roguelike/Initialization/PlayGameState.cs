using Roguelike.Input;
using Roguelike.Interaction;
using Roguelike.View;

namespace Roguelike.Initialization
{
    /// <summary>
    /// Represents a single play mode.
    /// </summary>
    public class PlayGameState : IGameState
    {
        private ILevelFactory levelFactory;

        public PlayGameState(string levelConfigPath)
        {
            levelFactory = new FileLevelFactory(levelConfigPath);
        }
        
        public PlayGameState()
        {
            levelFactory = new RandomLevelFactory();
        }

        public void InvokeState()
        {
            var level = levelFactory.CreateLevel();
            var inputLoop = new InputLoop();
            var playView = new ConsolePlayView();
            
            var moveInteractor = new MoveInteractor(level, playView);
            var exitGameInteractor = new ExitGameInteractor(inputLoop);
            
            var moveProcessor = new MoveProcessor(moveInteractor);
            var exitGameProcessor = new ExitGameProcessor(exitGameInteractor);
            
            var keyboardController = new KeyboardController();
            
            keyboardController.AddInputProcessor(moveProcessor);
            keyboardController.AddInputProcessor(exitGameProcessor);
            
            inputLoop.AddUpdatable(keyboardController);
            
            playView.Draw(level);
            inputLoop.Start();
        }
    }
}
