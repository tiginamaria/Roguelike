using Roguelike.Model.Objects;

namespace Roguelike.Model.Inventory
{
    /// <summary>
    /// A basic class for inventory items.
    /// Items can be activated and deactivated.
    /// Increase player's characteristics while activated.
    /// </summary>
    public abstract class InventoryItem : GameObject
    {
        private readonly int forceEffect;
        private readonly int healthEffect;
        private readonly int experienceEffect;

        public InventoryItem(Position position, int forceEffect, int healthEffect, int experienceEffect) : 
            base(position)
        {
            this.forceEffect = forceEffect;
            this.healthEffect = healthEffect;
            this.experienceEffect = experienceEffect;
        }

        /// <summary>
        /// Activates an item.
        /// </summary>
        public void Apply(CharacterStatistics statistics)
        {
            statistics.Force += forceEffect;
            statistics.Health += healthEffect;
            statistics.Experience += experienceEffect;
        }
        
        /// <summary>
        /// Deactivates an item.
        /// </summary>
        public void Remove(CharacterStatistics statistics)
        {
            statistics.Force -= forceEffect;
            statistics.Health -= healthEffect;
            statistics.Experience -= experienceEffect;
        }
    }
}