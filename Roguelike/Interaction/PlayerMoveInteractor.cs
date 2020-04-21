using Roguelike.Model;
using Roguelike.View;

namespace Roguelike.Interaction
{
    /// <summary>
    /// A class for performing player move logic.
    /// Notifies the view on update.
    /// </summary>
    public class PlayerMoveInteractor
    {
        private IPlayView playView;
        private Level level;

        public PlayerMoveInteractor(Level level, IPlayView playView)
        {
            this.playView = playView;
            this.level = level;
        }

        /// <summary>
        /// Notifies a player about the move intent.
        /// Notifies the view.
        /// </summary>
        public void IntentMove(int deltaY, int deltaX)
        {
            var player = level.Player;
            var oldPosition = player.Position;
            player.Move(deltaY, deltaX, level.Board);
            playView.UpdatePlayer(level, oldPosition, level.Player.Position);
        }
    }
}
