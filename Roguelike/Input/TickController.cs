using System.Collections.Generic;

namespace Roguelike.Input
{
    public class TickController : IUpdatable
    {
        private readonly List<ITickProcessor> subscribers = new List<ITickProcessor>();

        public void AddInputProcessor(ITickProcessor tickProcessor)
        {
            subscribers.Add(tickProcessor);
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
