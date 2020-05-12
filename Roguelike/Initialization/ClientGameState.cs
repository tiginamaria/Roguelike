using System;
using Roguelike.Input;
using Roguelike.Input.Controllers;
using Roguelike.Input.Processors;
using Roguelike.Interaction;
using Roguelike.View;

namespace Roguelike.Initialization
{
    public class ClientGameState : IGameState
    {
        private readonly ClientInputProcessor client;
        private readonly string login;
        private readonly int sessionId;

        public ClientGameState(ClientInputProcessor client, string login, int sessionId)
        {
            this.client = client;
            this.login = login;
            this.sessionId = sessionId;
        }

        public void InvokeState()
        {
            var playView = new ConsolePlayView();
            
            var level = client.Login(login, sessionId);

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

            level.CurrentPlayer = level.GetPlayer(login);
            
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
