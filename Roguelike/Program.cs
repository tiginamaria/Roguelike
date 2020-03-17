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
            var loop = new GameLoop();
            var factory = GetFactory(args);
            var startState = new PlayGameState(factory);
            loop.Run(startState);
        }
        
        private static ILevelFactory GetFactory(string[] args)
        {
            if (args.Length > 0)
            {
                return new FileLevelFactory(args[0]);
            }
            return new RandomLevelFactory();
        }
    }
}