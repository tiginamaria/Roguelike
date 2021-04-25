using Roguelike.Model;
using Roguelike.Model.PlayerModel;
using Roguelike.View;

namespace Roguelike.Interaction
{
    /// <summary>
    /// Performs a logic of player's inventory actions.
    /// </summary>
    public class InventoryInteractor
    {
        private readonly IPlayView playView;
        private readonly Level level;

        public InventoryInteractor(Level level, IPlayView playView)
        {
            this.level = level;
            this.playView = playView;
        }

        /// <summary>
        /// Activates the inventory item with the given type
        /// if it is a user-controlled player.
        /// </summary>
        public void PutOn(Character character, string inventoryType)
        {
            if (character is AbstractPlayer player)
            {
                player.PutOn(inventoryType);
            }

            if (level.IsCurrentPlayer(character))
            {
                playView.UpdateInventory(level);
            }
        }

        /// <summary>
        /// Deactivates the inventory item with the given type
        /// if it is a user-controlled player.
        /// </summary>
        public void PutOff(Character character, string inventoryType)
        {
            if (character is AbstractPlayer player)
            {
                player.PutOff(inventoryType);
            }

            if (level.IsCurrentPlayer(character))
            {
                playView.UpdateInventory(level);
            }
        }
    }
}