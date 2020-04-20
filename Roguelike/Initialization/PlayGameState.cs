using Roguelike.Input;
using Roguelike.Input.Controllers;
using Roguelike.Input.Processors;
using Roguelike.Interaction;
using Roguelike.View;

namespace Roguelike.Initialization
{
    /// <summary>
    /// Represents a single play mode.
    /// </summary>
    public class PlayGameState : IGameState
    {
        private readonly ILevelFactory levelFactory;

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
            
            var playerMoveInteractor = new PlayerMoveInteractor(level, playView);
            var mobMoveInteractor = new MobMoveInteractor(level, playView);
            var exitGameInteractor = new ExitGameInteractor(inputLoop);
            
            var moveProcessor = new MoveProcessor(playerMoveInteractor);
            var exitGameProcessor = new ExitGameProcessor(exitGameInteractor);

            var keyboardController = new KeyboardController();
            var tickController = new TickController();
            
            keyboardController.AddInputProcessor(moveProcessor);
            keyboardController.AddInputProcessor(exitGameProcessor);
            
            inputLoop.AddUpdatable(keyboardController);
            inputLoop.AddFixedUpdatable(tickController);

            var mobs = level.Mobs;
            foreach (var mob in mobs)
            {
                var mobMoveProcessor = new MobMoveProcessor(mob, mobMoveInteractor);
                tickController.AddTickProcessor(mobMoveProcessor);
                mob.OnDie += (sender, args) => { tickController.RemoveTickProcessor(mobMoveProcessor); };
            }

            level.Player.OnDie += (sender, args) => { inputLoop.Stop(); };

            playView.Draw(level);
            inputLoop.Start();
        }
    }
}
