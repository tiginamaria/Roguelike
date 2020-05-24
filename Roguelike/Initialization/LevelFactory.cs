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
        protected readonly InventoryFactory InventoryFactory = new InventoryFactory();
        protected MobFactory MobFactory = new MobFactory();
        protected PlayerFactory PlayerFactory = new PlayerFactory();
        
        /// <summary>
        /// Returns a new level.
        /// </summary>
        public abstract Level CreateLevel();

        /// <summary>
        /// Sets a new factory for mob creation instead of default.
        /// </summary>
        public void SetMobFactory(MobFactory mobFactory) => MobFactory = mobFactory;

        /// <summary>
        /// Sets a new factory for player creation instead of default.
        /// </summary>
        public void SetPlayerFactory(PlayerFactory playerFactory) => PlayerFactory = playerFactory;
    }
}
