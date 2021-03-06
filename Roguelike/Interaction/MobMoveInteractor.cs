using Roguelike.Input.Processors;
using Roguelike.Model;
using Roguelike.Model.Mobs;
using Roguelike.Model.PlayerModel;
using Roguelike.View;

namespace Roguelike.Interaction
{
    /// <summary>
    /// A class for performing mob's move logic.
    /// Notifies the view on update.
    /// </summary>
    public class MobMoveInteractor
    {
        protected readonly IPlayView playView;
        private readonly IMobMoveListener listener;
        protected readonly Level level;

        public MobMoveInteractor(Level level, IPlayView playView, IMobMoveListener listener = null)
        {
            this.playView = playView;
            this.listener = listener;
            this.level = level;
        }

        /// <summary>
        /// Notifies a mob about the move intent.
        /// Notifies the view.
        /// </summary>
        public virtual void IntentMove(Mob mob, int deltaY, int deltaX)
        {
            var oldPosition = mob.Position;
            mob.Move(deltaY, deltaX, level.Board);
            playView.UpdateMob(level, oldPosition, mob.Position);
            playView.UpdatePosition(level, oldPosition + new Position(deltaY, deltaX));

            var intentPosition = oldPosition + new Position(deltaY, deltaX);
            if (level.Board.GetObject(intentPosition) is AbstractPlayer)
            {
                playView.UpdateInventory(level);
            }

            if (deltaX != 0 || deltaY != 0)
            {
                listener?.Move(mob, new Position(deltaY, deltaX));
            }
        }
    }
}
