using System;

namespace Roguelike
{
    public static class Keyboard
    {
        private static ConsoleKey? key;
        
        public static void Update()
        {
            key = null;
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey().Key;
            }
        }
        
        public static ConsoleKey? GetKey()
        {
            return key;
        }
    }
}