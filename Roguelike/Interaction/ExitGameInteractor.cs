using Roguelike.Input;
using Roguelike.Model;

namespace Roguelike.Interaction
{
    public class ExitGameInteractor
    {
        private readonly IStoppable target;

        public ExitGameInteractor(IStoppable target)
        {
            this.target = target;
        }

        public void Exit(Character character)
        {
            // TODO
            target.Stop();
        }
    }
}
