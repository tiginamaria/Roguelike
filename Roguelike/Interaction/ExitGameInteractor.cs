using Roguelike.Input;

namespace Roguelike.Interaction
{
    public class ExitGameInteractor
    {
        private IStoppable target;
        
        public ExitGameInteractor(IStoppable target)
        {
            this.target = target;
        }

        public void Exit()
        {    
            target.Stop();
        }
    }
}
