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
        private readonly IPlayView playView;
        private readonly Level level;

        public PlayerMoveInteractor(Level level, IPlayView playView)
        {
            this.playView = playView;
            this.level = level;
        }

        /// <summary>
        /// Notifies a player about the move intent.
        /// Notifies the view.
        /// </summary>
        public void IntentMove(Character character, int deltaY, int deltaX)
        {
            var oldPosition = character.Position;
            character.Move(deltaY, deltaX, level.Board);

            if (level.IsCurrentPlayer(character))
            {
                playView.UpdatePlayer(level, oldPosition, character.Position);
            }
            else
            {
                playView.UpdateMob(level, oldPosition, character.Position);
                playView.UpdatePosition(level, oldPosition + new Position(deltaY, deltaX));
            }
        }
    }
}
