using System;
using System.IO;
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
        private readonly LevelFactory levelFactory;
        private const string Login = "OfflineUser";

        public PlayGameState(string arg)
        {
            if (arg == "--load")
            {
                if (!File.Exists(SaveGameInteractor.SaveFileName))
                {
                    throw new ArgumentException("Loading is impossible as dump file is missing.");
                }
                levelFactory = new FileLevelFactory(SaveGameInteractor.SaveFileName);
            }
            else
            {
                levelFactory = new FileLevelFactory(arg);    
            }
        }
        
        public PlayGameState()
        {
            levelFactory = new RandomLevelFactory();
        }

        ///<summary>
        /// Creates Input, View and Interaction components:
        /// Creates different move processors
        /// for each character on board, registers them in controllers.
        /// Creates interactors and passes created level and view to them.
        /// </summary>
        public void InvokeState()
        {
            var level = levelFactory.CreateLevel();
            var inputLoop = new InputLoop();
            var playView = new ConsolePlayView();
            
            var playerMoveInteractor = new PlayerMoveInteractor(level, playView);
            var mobMoveInteractor = new MobMoveInteractor(level, playView);
            var exitGameInteractor = new ExitGameInteractor(inputLoop);
            var saveGameInteractor = new SaveGameInteractor(level);
            var inventoryInteractor = new InventoryInteractor(level, playView);
            
            var moveProcessor = new MoveProcessor(playerMoveInteractor);
            var exitGameProcessor = new ExitGameProcessor(exitGameInteractor, saveGameInteractor);
            var saveGameProcessor = new SaveGameProcessor(saveGameInteractor);
            var inventoryProcessor = new InventoryProcessor(inventoryInteractor);

            var keyboardController = new KeyboardController(level, Login);
            var tickController = new TickController();
            
            keyboardController.AddInputProcessor(moveProcessor);
            keyboardController.AddInputProcessor(exitGameProcessor);
            keyboardController.AddInputProcessor(saveGameProcessor);
            keyboardController.AddInputProcessor(inventoryProcessor);
            
            inputLoop.AddUpdatable(keyboardController);
            inputLoop.AddFixedUpdatable(tickController);

            var mobs = level.Mobs;
            foreach (var mob in mobs)
            {
                var mobMoveProcessor = new MobMoveProcessor(mob, mobMoveInteractor);
                tickController.AddTickProcessor(mobMoveProcessor);
                mob.OnDie += (sender, args) => { tickController.RemoveTickProcessor(mobMoveProcessor); };
            }

            var player = level.AddPlayer(Login);
            level.CurrentPlayer = player;
            level.CurrentPlayer.OnDie += (sender, args) =>
            {
                inputLoop.Stop();
                saveGameInteractor.Delete();
            };

            playView.Draw(level);
            inputLoop.Start();
        }
    }
}
