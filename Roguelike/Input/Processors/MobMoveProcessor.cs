using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.Mobs;

namespace Roguelike.Input.Processors
{
    /// <summary>
    /// A class for generating mob moves every tick.
    /// </summary>
    public class MobMoveProcessor : ITickProcessor
    {
        private readonly Mob mob;
        private readonly MobMoveInteractor interactor;
        private readonly IMobMoveListener listener;

        public MobMoveProcessor(Mob mob, MobMoveInteractor interactor, IMobMoveListener listener = null)
        {
            this.mob = mob;
            this.interactor = interactor;
            this.listener = listener;
        }
        
        /// <summary>
        /// Receives the move from the mob, passes it to the interactor.
        /// </summary>
        public void ProcessTick()
        {
            var oldPosition = mob.Position;
            var newPosition = mob.GetMove();
            var delta = newPosition - oldPosition;
            interactor.IntentMove(mob, delta.Y, delta.X);

            if (delta != new Position(0, 0))
            {
                listener?.Move(mob, delta);
            }
        }
    }
}
