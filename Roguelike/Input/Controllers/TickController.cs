using System.Collections.Generic;
using Roguelike.Input.Processors;

namespace Roguelike.Input.Controllers
{
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
