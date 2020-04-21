using System.Collections.Generic;
using Roguelike.Input.Processors;

namespace Roguelike.Input.Controllers
{
    /// <summary>
    /// Handles ticks and notifies the subscribed processors.
    /// </summary>
    public class TickController : IUpdatable
    {
        private readonly List<ITickProcessor> subscribers = new List<ITickProcessor>();

        public void AddTickProcessor(ITickProcessor tickProcessor)
        {
            subscribers.Add(tickProcessor);
        }

        public void RemoveTickProcessor(ITickProcessor processor)
        {
            subscribers.Remove(processor);
        }

        /// <summary>
        /// Notifies the subscribed processors.
        /// </summary>
        public void Update()
        {
            var oldSubscribers = new List<ITickProcessor>(subscribers);
            foreach (var subscriber in oldSubscribers)
            {
                subscriber.ProcessTick();
            }
        }
    }
}
