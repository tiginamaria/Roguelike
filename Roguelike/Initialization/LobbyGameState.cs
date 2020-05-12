using System;

namespace Roguelike.Initialization
{
    public class LobbyGameState : IGameState
    {
        private readonly StateManager stateManager;
        
        public LobbyGameState()
        {
            stateManager = StateManager.GetInstance();
        }
        
        public void InvokeState()
        {
            while (true)
            {
                Console.Write("Input login: ");
                var login = Console.ReadLine();

                try
                {
                    var clientGameState = new ClientGameState(login);
                    stateManager.ChangeState(clientGameState);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
