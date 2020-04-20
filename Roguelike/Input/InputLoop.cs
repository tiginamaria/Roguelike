using System;
using System.Collections.Generic;
using System.Timers;

namespace Roguelike.Input
{
    /// <summary>
    /// Updates the input sources of the game.
    /// </summary>
    public class InputLoop : IStoppable
    {
        private readonly List<IUpdatable> subscribers = new List<IUpdatable>();
        private readonly List<IUpdatable> fixedSubscribers = new List<IUpdatable>();
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

            timer = new Timer(FixUpdatePeriodMillis);
            timer.Elapsed += NotifyFixedSubscribers;
            timer.Start();

            while (!stopped)
            {
                foreach (var subscriber in subscribers) 
                {
                    subscriber.Update();
                }
            }
            
            OnExit?.Invoke(this,EventArgs.Empty);
        }

        private void NotifyFixedSubscribers(object sender, ElapsedEventArgs args)
        {
            foreach (var subscriber in fixedSubscribers) 
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
