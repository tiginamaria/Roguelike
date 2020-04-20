using Roguelike.Model;
using Roguelike.View;

namespace Roguelike.Interaction
{
    /// <summary>
    /// A class for performing player move logic.
    /// Notifies the view on update.
    /// </summary>
    public class MoveInteractor
    {
        private Player player;
        private IPlayView playView;
        private Level level;

        public MoveInteractor(Level level, IPlayView playView)
        {
            player = level.Player;
            this.playView = playView;
            this.level = level;
        }

        /// <summary>
        /// Notifies a user about the move intent.
        /// Notifies the view.
        /// </summary>
        public void IntentMove(int deltaY, int deltaX)
        {
            player.Move(deltaY, deltaX, level.Board);
            playView.Draw(level);
        }
    }
}
