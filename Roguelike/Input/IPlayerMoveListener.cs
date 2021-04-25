using Roguelike.Model;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Input
{
    public interface IPlayerMoveListener
    {
        /// <summary>
        /// Observes a player move.
        /// </summary>
        void MovePlayer(AbstractPlayer player, Position intentPosition);
    }
}