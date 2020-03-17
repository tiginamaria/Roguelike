namespace Roguelike
{
    /// <summary>
    /// A class for performing player move logic.
    /// Notifies the view on update.
    /// </summary>
    public class MoveInteractor
    {
        private Player player;
        private ConsolePlayView playView;
        private Level level;

        public MoveInteractor(Level level, ConsolePlayView playView)
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