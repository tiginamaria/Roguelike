using Roguelike.Input;
using Roguelike.Input.Controllers;
using Roguelike.Input.Processors;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.PlayerModel;
using Roguelike.Network;
using Roguelike.View;

namespace Roguelike.Initialization
{
    public class ServerGameState : IGameState
    {
        private readonly LevelFactory levelFactory;
        private const string DummyLogin = "Dummy";

        public ServerGameState()
        {
            levelFactory = new RandomLevelFactory();
        }

        public void InvokeState()
        {
            var level = levelFactory.CreateLevel();
            var inputLoop = new InputLoop();
            var playView = new VoidView();
            
            var inputController = new ServerInputController(level);
            
            var playerMoveInteractor = new PlayerMoveInteractor(level, playView, inputController);
            var mobMoveInteractor = new MobMoveInteractor(level, playView, inputController);
           
            //TODO
            var exitGameInteractor = new ExitGameInteractor(level, inputController);
            
            var inventoryInteractor = new InventoryInteractor(level, playView);
            
            var moveProcessor = new MoveProcessor(playerMoveInteractor);
            var exitGameProcessor = new ExitGameProcessor(exitGameInteractor);
            var inventoryProcessor = new InventoryProcessor(inventoryInteractor, inputController);

            
            var tickController = new TickController();
            
            inputController.AddInputProcessor(moveProcessor);
            //keyboardController.AddInputProcessor(exitGameProcessor);
            inputController.AddInputProcessor(inventoryProcessor);
            inputController.AddInputProcessor(exitGameProcessor);
            
            inputLoop.AddFixedUpdatable(tickController);
            inputLoop.AddUpdatable(inputController);

            level.CurrentPlayer = new Player(DummyLogin, level, new Position(-1, -1));
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

            var server = new NetworkServer(inputController);
            server.Start();
            
            inputLoop.Start();
        }
    }
}
