namespace Roguelike
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var loop = new GameLoop();
            var factory = GetFactory(args);
            var startState = new PlayGameState(factory);
            loop.Start(startState);
        }
        
        private static ILevelFactory GetFactory(string[] args)
        {
            return new RandomLevelFactory();
        }
    }
}