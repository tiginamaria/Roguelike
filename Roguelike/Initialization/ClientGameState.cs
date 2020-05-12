using System;
using Roguelike.Input;
using Roguelike.Input.Controllers;
using Roguelike.Input.Processors;
using Roguelike.Interaction;
using Roguelike.Model.PlayerModel;
using Roguelike.View;

namespace Roguelike.Initialization
{
    public class ClientGameState : IGameState
    {
        private readonly string login;

        public ClientGameState(string login)
        {
            this.login = login;
        }

        public void InvokeState()
        {
            var playView = new ConsolePlayView();
            
            var client = new ClientInputProcessor(playView);
            var level = client.Login(login);

            if (level == null)
            {
                throw new ArgumentException("Login already exists.");
            }
            
            var inputLoop = new InputLoop();
            
            var playerMoveInteractor = new NetworkPlayerMoveInteractor(level, playView);
            var mobMoveInteractor = new NetworkMobMoveInteractor(level, playView);
            var exitGameInteractor = new ExitGameInteractor(level);
            var saveGameInteractor = new SaveGameInteractor(level);
            var inventoryInteractor = new InventoryInteractor(level, playView);
            var spawnPlayerInteractor = new SpawnPlayerInteractor(level, playView);
            
            var moveProcessor = new MoveProcessor(playerMoveInteractor);
            var exitGameProcessor = new ExitGameProcessor(exitGameInteractor);
            var saveGameProcessor = new SaveGameProcessor(saveGameInteractor);
            var inventoryProcessor = new InventoryProcessor(inventoryInteractor);

            var keyboardController = new KeyboardController(level, login);
            keyboardController.AddInputProcessor(client);
            
            keyboardController.AddInputProcessor(exitGameProcessor);
            keyboardController.AddInputProcessor(saveGameProcessor);
            
            client.AddInputProcessor(moveProcessor);
            client.AddInputProcessor(exitGameProcessor);
            client.AddInputProcessor(inventoryProcessor);
            
            client.SetMobInteractor(mobMoveInteractor);
            client.SetPlayerMoveInteractor(playerMoveInteractor);
            client.SetSpawnPlayerInteractor(spawnPlayerInteractor);
            
            inputLoop.AddUpdatable(keyboardController);
            inputLoop.AddUpdatable(client);

            level.CurrentPlayer = level.GetCharacter(login) as AbstractPlayer;
            
            level.CurrentPlayer.OnDie += (sender, args) =>
            {
                inputLoop.Stop();
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
