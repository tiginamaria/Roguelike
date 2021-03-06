using System;
using Roguelike.Model;

namespace Roguelike.Input.Processors
{
    /// <summary>
    /// An interface for handling user input.
    /// </summary>
    public interface IInputProcessor
    {
        /// <summary>
        /// This method is called whenever the user input is handled.
        /// </summary>
        /// <param name="key">A key pressed</param>
        /// <param name="character"></param>
        void ProcessInput(ConsoleKeyInfo key, Character character);
    }
}
