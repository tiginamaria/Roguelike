using System;
using System.Collections.Generic;
using Roguelike.Input.Processors;

namespace Roguelike.Input.Controllers
{
    /// <summary>
    /// Wrapper class for the keyboard input.
    /// </summary>
    public class KeyboardController : IUpdatable
    {
        private readonly List<IInputProcessor> subscribers = new List<IInputProcessor>();

        public void AddInputProcessor(IInputProcessor inputProcessor)
        {
            subscribers.Add(inputProcessor);
        }

        /// <summary>
        /// Updates the input in non-blocking mode.
        /// Notifies the subscribers.
        /// </summary>
        public void Update()
        {
            if (!Console.KeyAvailable)
            {
                return;
            }
            
            var key = Console.ReadKey().Key;
            foreach (var subscriber in subscribers)
            {
                subscriber.ProcessInput(key);
            }
        }
    }
}
