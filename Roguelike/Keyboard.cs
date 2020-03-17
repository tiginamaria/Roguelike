using System;

namespace Roguelike
{
    /// <summary>
    /// Wrapper class for the keyboard input.
    /// Stores the key into a static buffer.
    /// It allows analyzing it from different places (classes-controllers).
    /// Otherwise calling Console.ReadKey() from one controller will consume the key
    /// and the next controller will not get it.
    /// </summary>
    public static class Keyboard
    {
        private static ConsoleKey? key;
        
        /// <summary>
        /// Updates the input in non-blocking mode.
        /// </summary>
        public static void Update()
        {
            key = null;
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey().Key;
            }
        }
        
        /// <summary>
        /// Returns the key from the last update.
        /// </summary>
        public static ConsoleKey? GetKey()
        {
            return key;
        }
    }
}