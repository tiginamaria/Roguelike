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
            
            var client = new ClientInputController(playView);
            var level = client.Login(login);

            if (level == null)
            {
                throw new ArgumentException("Login already exists.");
            }
            
            var inputLoop = new InputLoop();
            
            var playerMoveInteractor = new PlayerMoveInteractor(level, playView);
            var mobMoveInteractor = new MobMoveInteractor(level, playView);
            var exitGameInteractor = new ExitGameInteractor(inputLoop);
            var saveGameInteractor = new SaveGameInteractor(level);
            var inventoryInteractor = new InventoryInteractor(level, playView);
            
            var moveProcessor = new MoveProcessor(playerMoveInteractor);
            var exitGameProcessor = new ExitGameProcessor(exitGameInteractor, saveGameInteractor);
            var saveGameProcessor = new SaveGameProcessor(saveGameInteractor);
            var inventoryProcessor = new InventoryProcessor(inventoryInteractor);

            var keyboardController = new KeyboardController(level, login);
            keyboardController.AddInputProcessor(client);
            //var tickController = new TickController();
            
            // keyboardController.AddInputProcessor(moveProcessor);
            // keyboardController.AddInputProcessor(exitGameProcessor);
            keyboardController.AddInputProcessor(saveGameProcessor);
            // keyboardController.AddInputProcessor(inventoryProcessor);
            
            client.AddInputProcessor(moveProcessor);
            client.AddInputProcessor(exitGameProcessor);
            client.AddInputProcessor(inventoryProcessor);
            
            client.SetMobInteractor(mobMoveInteractor);
            
            inputLoop.AddUpdatable(keyboardController);
            //inputLoop.AddFixedUpdatable(tickController);
            inputLoop.AddUpdatable(client);

            level.CurrentPlayer = level.GetCharacter(login) as AbstractPlayer;
            
            //TODO
            // level.CurrentPlayer.OnDie += (sender, args) =>
            // {
            //     inputLoop.Stop();
            //     saveGameInteractor.Delete();
            // };

            playView.Draw(level);
            inputLoop.Start();
        }
    }
}
