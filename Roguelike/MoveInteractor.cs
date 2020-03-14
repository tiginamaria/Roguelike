namespace Roguelike
{
    public class MoveInteractor
    {
        private Player player;
        private PlayView playView;
        private Level level;

        public MoveInteractor(Level level, PlayView playView)
        {
            player = level.Player;
            this.playView = playView;
            this.level = level;
        }

        public void IntentMove(int deltaY, int deltaX)
        {
            player.Move(deltaY, deltaX, level.Board);
            playView.Draw(level);
        }
    }
}