using System;
using System.Collections.Generic;
using Roguelike.Input.Processors;
using Roguelike.Model;

namespace Roguelike.Input.Controllers
{
    /// <summary>
    /// Wrapper class for the keyboard input.
    /// </summary>
    public class KeyboardController : IUpdatable
    {
        private readonly List<IInputProcessor> subscribers = new List<IInputProcessor>();
        private readonly Level level;
        private readonly string login;
        private bool stopped;

        public KeyboardController(Level level, string login)
        {
            this.level = level;
            this.login = login;
        }

        public void AddInputProcessor(IInputProcessor inputProcessor)
        {
            subscribers.Add(inputProcessor);
        }

        public void Stop()
        {
            stopped = true;
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
            var key = Console.ReadKey(true);

            foreach (var subscriber in subscribers)
            {
                if (stopped)
                {
                    break;
                }
                subscriber.ProcessInput(key, level.GetCharacter(login));
            }
        }
    }
}
