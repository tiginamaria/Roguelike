using System;

namespace Roguelike.Input.Processors
{
    /// <summary>
    /// An interface for processing user input.
    /// </summary>
    public interface IInputProcessor
    {
        /// <summary>
        /// This method is called whenever the user input is handled.
        /// </summary>
        /// <param name="key">A key pressed</param>
        void ProcessInput(ConsoleKeyInfo key);
    }
}
