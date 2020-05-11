using Roguelike.Model;
using Roguelike.Model.PlayerModel;
using Roguelike.View;

namespace Roguelike.Interaction
{
    public class InventoryInteractor
    {
        private readonly IPlayView playView;
        private readonly Level level;

        public InventoryInteractor(Level level, IPlayView playView)
        {
            this.level = level;
            this.playView = playView;
        }

        public void PutOn(Character character, string inventoryType)
        {
            if (character is Player player)
            {
                player.PutOn(inventoryType);
            }

            if (level.IsCurrentPlayer(character))
            {
                playView.UpdateInventory(level);
            }
        }

        public void PutOff(Character character, string inventoryType)
        {
            if (character is Player player)
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