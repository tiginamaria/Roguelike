using Roguelike.Model;
using Roguelike.Model.Inventory;
using Roguelike.Model.Mobs;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Initialization
{
    /// <summary>
    /// Interface for level factories. 
    /// </summary>
    public abstract class LevelFactory
    {
        protected static readonly InventoryFactory InventoryFactory = new InventoryFactory();
        protected static readonly MobFactory MobFactory = new MobFactory();
        protected static readonly AbstractPlayerFactory PlayerFactory = new AbstractPlayerFactory();
        /// <summary>
        /// Returns a new level.
        /// </summary>
        public abstract Level CreateLevel();

        public virtual AbstractPlayer AddPlayer(string login)
        {
            return null;
        }
    }
}
