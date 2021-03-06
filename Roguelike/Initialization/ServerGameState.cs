using Roguelike.Input;
using Roguelike.Input.Controllers;
using Roguelike.Input.Processors;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.PlayerModel;
using Roguelike.Network.Services;
using Roguelike.View;

namespace Roguelike.Initialization
{
    /// <summary>
    /// Sets up the game on the server side in a network mode.
    /// </summary>
    public class ServerGameState : IGameState
    {
        private readonly int newSessionId;
        private readonly ServerInputService inputService;
        private readonly LevelFactory levelFactory;
        private const string DummyLogin = "Dummy";

        public ServerGameState(int newSessionId, ServerInputService inputService)
        {
            this.newSessionId = newSessionId;
            this.inputService = inputService;
            levelFactory = new RandomLevelFactory();
        }

        public void InvokeState()
        {
            var level = levelFactory.CreateLevel();
            var inputLoop = new InputLoop();
            var playView = new VoidView();

            var inputController = new ServerInputController(level, newSessionId, inputService);
            
            var exitGameInteractor = new ExitGameInteractor(level, inputController);
            var playerMoveInteractor = new PlayerMoveInteractor(level, playView, inputController);
            var mobMoveInteractor = new MobMoveInteractor(level, playView, inputController);
            var inventoryInteractor = new InventoryInteractor(level, playView);
            
            levelFactory.SetPlayerFactory(new NetworkPlayerFactory(exitGameInteractor));
            
            var moveProcessor = new MoveProcessor(playerMoveInteractor);
            var exitGameProcessor = new ExitGameProcessor(exitGameInteractor);
            var inventoryProcessor = new InventoryProcessor(inventoryInteractor, inputController);
            
            var tickController = new TickController();
            
            inputController.AddInputProcessor(moveProcessor);
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
                mob.OnDie += (sender, args) =>
                {
                    level.Mobs.Remove(mob);
                    tickController.RemoveTickProcessor(mobMoveProcessor);
                };
            }

            inputLoop.Start();
        }
    }
}
