using Roguelike.Model;
using Roguelike.Model.Mobs;
using Roguelike.Model.PlayerModel;
using Roguelike.View;

namespace Roguelike.Interaction
{
    /// <summary>
    /// A specific interactor for network mode.
    /// Moves a player whenever it is confused or not. 
    /// </summary>
    public class NetworkMobMoveInteractor : MobMoveInteractor
    {
        public NetworkMobMoveInteractor(Level level, IPlayView playView) : base(level, playView)
        {
        }
        
        public override void IntentMove(Mob mob, int deltaY, int deltaX)
        {
            if (mob == null)
            {
                return;
            }
            
            var oldPosition = mob.Position;
            mob.MoveStraightforward(deltaY, deltaX, level.Board);
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