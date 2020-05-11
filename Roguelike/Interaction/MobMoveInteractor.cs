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
        private readonly IPlayView playView;
        private readonly Level level;

        public MobMoveInteractor(Level level, IPlayView playView)
        {
            this.playView = playView;
            this.level = level;
        }

        /// <summary>
        /// Notifies a mob about the move intent.
        /// Notifies the view.
        /// </summary>
        public void IntentMove(Mob mob, int deltaY, int deltaX)
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
        }
    }
}
