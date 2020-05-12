using Roguelike.Interaction;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Input
{
    public interface IActionListener
    {
        void MakeAction(AbstractPlayer player, ActionType actionType);
    }
}