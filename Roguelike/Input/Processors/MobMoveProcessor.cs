using Roguelike.Interaction;
using Roguelike.Model.Mobs;

namespace Roguelike.Input.Processors
{
    public class MobMoveProcessor : ITickProcessor
    {
        private readonly Mob mob;
        private readonly MobMoveInteractor interactor;
        
        public MobMoveProcessor(Mob mob, MobMoveInteractor interactor)
        {
            this.mob = mob;
            this.interactor = interactor;
        }
        
        public void ProcessTick()
        {
            var oldPosition = mob.Position;
            var newPosition = mob.GetMove();
            var delta = newPosition - oldPosition;
            interactor.IntentMove(mob, delta.Y, delta.X);
        }
    }
}
