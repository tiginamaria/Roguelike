using Roguelike.Model;
using Roguelike.Model.Inventory;

namespace Roguelike.Initialization
{
    /// <summary>
    /// Interface for level factories. 
    /// </summary>
    public abstract class ILevelFactory
    {
        private static InventoryFactory InventoryFactory = new InventoryFactory();
        /// <summary>
        /// Returns a new level.
        /// </summary>
        public abstract Level CreateLevel();
    }
}
