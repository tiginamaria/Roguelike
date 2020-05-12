using Roguelike.Model;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Input
{
    public interface IPlayerMoveListener
    {
        void MovePlayer(AbstractPlayer player, Position intentPosition);
    }
}