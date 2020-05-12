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

        public void SetMobFactory(MobFactory mobFactory)
        {
            MobFactory = mobFactory;
        }

        public void SetPlayerFactory(PlayerFactory playerFactory)
        {
            PlayerFactory = playerFactory;
        }
    }
}
