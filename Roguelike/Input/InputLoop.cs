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
        private readonly List<IUpdatable> subscribers = new List<IUpdatable>();
        private readonly List<IUpdatable> fixedSubscribers = new List<IUpdatable>();
        private bool stopped;
        private Timer timer;
        private const float FixUpdatePeriodMillis = 500;

        /// <summary>
        /// Registers an object that will be updated infinitely.
        /// </summary>
        public void AddUpdatable(IUpdatable updatable) => subscribers.Add(updatable);
        
        /// <summary>
        /// Registers an object that will be updated by timer.
        /// </summary>

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
        }

        private void Notify(IEnumerable<IUpdatable> targetSubscribers)
        {
            foreach (var subscriber in targetSubscribers) 
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
