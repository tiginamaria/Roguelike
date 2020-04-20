using System;
using System.Collections.Generic;
using System.Timers;
using Roguelike.Input.Controllers;

namespace Roguelike.Input
{
    /// <summary>
    /// Updates the input sources of the game.
    /// </summary>
    public class InputLoop : IStoppable
    {
        private List<IUpdatable> subscribers = new List<IUpdatable>();
        private List<IUpdatable> fixedSubscribers = new List<IUpdatable>();
        private bool stopped;
        private Timer timer;
        private const float FixUpdatePeriodMillis = 500;

        public event EventHandler OnExit;
        
        public void AddUpdatable(IUpdatable updatable) => subscribers.Add(updatable);

        public void AddFixedUpdatable(IUpdatable updatable) => fixedSubscribers.Add(updatable);

        /// <summary>
        /// Starts an infinite loop and updates all subscribers.
        /// </summary>
        public void Start()
        {
            stopped = false;
            var fixedUpdate = false;
            
            timer = new Timer(FixUpdatePeriodMillis);
            timer.Elapsed += (sender, args) => fixedUpdate = true;
            timer.Start();

            while (!stopped)
            {
                Notify(subscribers);

                if (fixedUpdate)
                {
                    fixedUpdate = false;
                    Notify(fixedSubscribers);
                }
            }
            
            OnExit?.Invoke(this,EventArgs.Empty);
        }

        private void Notify(List<IUpdatable> targetSubscribers)
        {
            var oldSubscribers = new List<IUpdatable>(targetSubscribers);
            foreach (var subscriber in oldSubscribers) 
            {
                subscriber.Update();
            }
        }

        /// <summary>
        /// Stops the loop.
        /// </summary>
        public void Stop()
        {
            timer.Stop();
            stopped = true;
        }
    }
}
