using Roguelike.Input;
using Roguelike.Input.Controllers;
using Roguelike.Input.Processors;
using Roguelike.Interaction;
using Roguelike.Network;
using Roguelike.View;

namespace Roguelike.Initialization
{
    public class ServerGameState : IGameState
    {
        private readonly ILevelFactory levelFactory;

        public ServerGameState()
        {
            levelFactory = new RandomLevelFactory();
        }

        public void InvokeState()
        {
            var level = levelFactory.CreateLevel();
            var inputLoop = new InputLoop();
            var playView = new VoidView();
            
            var playerMoveInteractor = new PlayerMoveInteractor(level, playView);
            var mobMoveInteractor = new MobMoveInteractor(level, playView);
           
            //TODO
            // var exitGameInteractor = new ExitGameInteractor(inputLoop);
            
            var inventoryInteractor = new InventoryInteractor(level, playView);
            
            var moveProcessor = new MoveProcessor(playerMoveInteractor);
            //var exitGameProcessor = new ExitGameProcessor(exitGameInteractor, saveGameInteractor);
            var inventoryProcessor = new InventoryProcessor(inventoryInteractor);

            var keyboardController = new ServerInputController(level);
            var tickController = new TickController();
            
            keyboardController.AddInputProcessor(moveProcessor);
            //keyboardController.AddInputProcessor(exitGameProcessor);
            keyboardController.AddInputProcessor(inventoryProcessor);
            
            inputLoop.AddFixedUpdatable(tickController);

            var mobs = level.Mobs;
            foreach (var mob in mobs)
            {
                var mobMoveProcessor = new MobMoveProcessor(mob, mobMoveInteractor);
                tickController.AddTickProcessor(mobMoveProcessor);
                mob.OnDie += (sender, args) => { tickController.RemoveTickProcessor(mobMoveProcessor); };
            }

            // level.Player.OnDie += (sender, args) =>
            // {
            //     inputLoop.Stop();
            // };

            var server = new NetworkServer(keyboardController);
            server.Start();
            
            inputLoop.Start();
        }
    }
}
