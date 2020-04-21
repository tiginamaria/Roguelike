using System.Collections.Generic;
using Roguelike.Input.Processors;

namespace Roguelike.Input.Controllers
{
    /// <summary>
    /// Handles ticks and notifies the subscribed processors.
    /// </summary>
    public class TickController : IUpdatable
    {
        private readonly HashSet<ITickProcessor> subscribers = new HashSet<ITickProcessor>();
        private readonly HashSet<ITickProcessor> removedSubscribers = new HashSet<ITickProcessor>();

        public void AddTickProcessor(ITickProcessor tickProcessor)
        {
            subscribers.Add(tickProcessor);
        }

        public void RemoveTickProcessor(ITickProcessor processor)
        {
            removedSubscribers.Add(processor);
        }

        /// <summary>
        /// Notifies the subscribed processors.
        /// </summary>
        public void Update()
        {
            foreach (var subscriber in subscribers)
            {
                if (!removedSubscribers.Contains(subscriber))
                {
                    subscriber.ProcessTick();
                }
            }

            foreach (var subscriber in removedSubscribers)
            {
                subscribers.Remove(subscriber);
            }
        }
    }
}
