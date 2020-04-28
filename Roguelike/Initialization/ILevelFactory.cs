using Roguelike.Model;
using Roguelike.Model.Inventory;
using Roguelike.Model.Mobs;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Initialization
{
    /// <summary>
    /// Interface for level factories. 
    /// </summary>
    public abstract class ILevelFactory
    {
        protected static InventoryFactory inventoryFactory = new InventoryFactory();
        protected static MobFactory mobFactory = new MobFactory();
        protected static AbstractPlayerFactory playerFactory = new AbstractPlayerFactory();
        /// <summary>
        /// Returns a new level.
        /// </summary>
        public abstract Level CreateLevel();
    }
}
