using Roguelike.Input.Controllers;
using Roguelike.Model;
using Roguelike.Model.PlayerModel;
using Roguelike.View;

namespace Roguelike.Interaction
{
    /// <summary>
    /// A class for performing player move logic.
    /// Notifies the view on update.
    /// </summary>
    public class PlayerMoveInteractor
    {
        protected readonly IPlayView playView;
        private readonly IPlayerMoveListener listener;
        protected readonly Level level;

        public PlayerMoveInteractor(Level level, IPlayView playView, IPlayerMoveListener listener = null)
        {
            this.playView = playView;
            this.listener = listener;
            this.level = level;
        }

        /// <summary>
        /// Notifies a player about the move intent.
        /// Notifies the view.
        /// </summary>
        public virtual void IntentMove(Character character, int deltaY, int deltaX)
        {
            var player = character as AbstractPlayer;

            var oldPosition = player.Position;

            if (player is ConfusedPlayer confusedPlayer)
            {
                var confusedPosition = confusedPlayer.ConfuseIntent(new Position(deltaY, deltaX));
                deltaY = confusedPosition.Y;
                deltaX = confusedPosition.X;
            }
            
            character.MoveStraightforward(deltaY, deltaX, level.Board);

            playView.UpdatePlayer(level, oldPosition, player.Position);
            playView.UpdatePosition(level, oldPosition + new Position(deltaY, deltaX));
            
            listener?.MovePlayer(player, new Position(deltaY, deltaX));
        }
    }
}
