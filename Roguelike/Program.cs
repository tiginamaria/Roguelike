using System;

namespace Roguelike
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var loop = new GameLoop();
            loop.Start();
        }
    }
}