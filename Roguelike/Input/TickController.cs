using System.Collections.Generic;

namespace Roguelike.Input
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
            foreach (var subscriber in subscribers)
            {
                subscriber.ProcessTick();
            }
        }
    }
}
