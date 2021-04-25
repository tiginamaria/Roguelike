using System;
using System.IO;
using Roguelike.Input;
using Roguelike.Input.Controllers;
using Roguelike.Input.Processors;
using Roguelike.Interaction;
using Roguelike.Model.PlayerModel;
using Roguelike.View;

namespace Roguelike.Initialization
{
    /// <summary>
    /// Represents a single play mode.
    /// </summary>
    public class OfflinePlayGameState : IGameState
    {
        private readonly LevelFactory levelFactory;
        private const string OfflinePlayerLogin = "OfflinePlayer";

        public OfflinePlayGameState(string arg)
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
        
        public OfflinePlayGameState()
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
            var saveGameInteractor = new SaveGameInteractor(level);
            var exitGameInteractor = new ExitGameInteractor(level);
            var inventoryInteractor = new InventoryInteractor(level, playView);
            
            var moveProcessor = new MoveProcessor(playerMoveInteractor);
            var saveAndExitGameProcessor = new SaveAndExitGameProcessor(exitGameInteractor, saveGameInteractor);
            var inventoryProcessor = new InventoryProcessor(inventoryInteractor);

            var keyboardController = new KeyboardController(level, OfflinePlayerLogin);
            var tickController = new TickController();
            
            keyboardController.AddInputProcessor(moveProcessor);
            keyboardController.AddInputProcessor(saveAndExitGameProcessor);
            keyboardController.AddInputProcessor(inventoryProcessor);
            
            inputLoop.AddUpdatable(keyboardController);
            inputLoop.AddFixedUpdatable(tickController);
            
            if (!level.ContainsPlayer(OfflinePlayerLogin))
            {
                level.AddPlayerAtEmpty(OfflinePlayerLogin);
            }

            level.CurrentPlayer = level.GetPlayer(OfflinePlayerLogin);
            
            var mobs = level.Mobs;
            foreach (var mob in mobs)
            {
                var mobMoveProcessor = new MobMoveProcessor(mob, mobMoveInteractor);
                tickController.AddTickProcessor(mobMoveProcessor);
                mob.OnDie += (sender, args) =>
                {
                    level.Mobs.Remove(mob);
                    tickController.RemoveTickProcessor(mobMoveProcessor);
                };
            }

            level.CurrentPlayer.OnDie += (sender, args) =>
            {
                inputLoop.Stop();
                saveGameInteractor.DeleteSaving();
            };

            exitGameInteractor.OnExit += (sender, player) =>
            {
                inputLoop.Stop();
            };

            playView.Draw(level);
            inputLoop.Start();
        }
    }
}
