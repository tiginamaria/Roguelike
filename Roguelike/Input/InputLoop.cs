using System;
using System.Collections.Generic;

namespace Roguelike.Input
{
    /// <summary>
    /// Updates the input sources of the game.
    /// </summary>
    public class InputLoop : IStoppable
    {
        private readonly List<IUpdatable> subscribers = new List<IUpdatable>();
        private bool stopped;

        public event EventHandler OnExit;
        
        public void AddUpdatable(IUpdatable updatable) => subscribers.Add(updatable);

        /// <summary>
        /// Starts an infinite loop and updates all subscribers.
        /// </summary>
        public void Start()
        {
            stopped = false;

            while (!stopped)
            {
                foreach (var subscriber in subscribers) 
                {
                    subscriber.Update();
                }
            }
            
            OnExit?.Invoke(this,EventArgs.Empty);
        }

        /// <summary>
        /// Stops the loop.
        /// </summary>
        public void Stop() => stopped = true;
    }
}