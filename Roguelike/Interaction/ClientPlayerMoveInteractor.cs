using Roguelike.Model;
using Roguelike.View;

namespace Roguelike.Interaction
{
    /// <summary>
    /// A specific interactor for network mode.
    /// Moves a mob whenever it is confused or not. 
    /// </summary>
    public class NetworkPlayerMoveInteractor : PlayerMoveInteractor
    {
        public NetworkPlayerMoveInteractor(Level level, IPlayView playView) : base(level, playView)
        {
        }
        
        public override void IntentMove(Character character, int deltaY, int deltaX)
        {
            var oldPosition = character.Position;
            character.MoveStraightforward(deltaY, deltaX, level.Board);

            if (level.IsCurrentPlayer(character))
            {
                playView.UpdatePlayer(level, oldPosition, character.Position);
            }
            else
            {
                playView.UpdateMob(level, oldPosition, character.Position);
            }
            playView.UpdatePosition(level, oldPosition + new Position(deltaY, deltaX));
        }
    }
}