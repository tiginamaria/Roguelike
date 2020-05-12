using System;
using Roguelike.Initialization;
using Roguelike.Network;

namespace Roguelike
{
    /// <summary>
    /// An entry point to the game.
    /// If no parameters are passed, a random level is generated.
    /// Otherwise, the first parameter is considered a path to the file
    /// with the level information.
    /// </summary>
    internal class Program
    {
        public static void Main(string[] args)
        {
            var startState = GetStartState(args);
            var stateManager = StateManager.GetInstance();
            stateManager.ChangeState(startState);
        }
        
        private static IGameState GetStartState(string[] args)
        {
            if (args.Length ==  0)
            {
                return new PlayGameState();
            }

            if (args[0] == "--load")
            {
                return new PlayGameState(args[0]);
            }

            if (args[0] == "--server")
            {
                return new NetworkServer();
            }

            if (args[0] == "--client")
            {
                return new LobbyGameState();
            }
            
            throw new ArgumentException("Invalid command-line argument", args[0]);
        }
    }
}
