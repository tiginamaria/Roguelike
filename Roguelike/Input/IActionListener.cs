using Roguelike.Interaction;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Input
{
    public interface IActionListener
    {
        /// <summary>
        /// Observes a player action (i.e. activating inventory item).
        /// </summary>
        void MakeAction(AbstractPlayer player, ActionType actionType);
    }
}