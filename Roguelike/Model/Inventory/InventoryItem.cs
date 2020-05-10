using Roguelike.Model.Objects;

namespace Roguelike.Model.Inventory
{
    public abstract class InventoryItem: GameObject
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

        public void Apply(CharacterStatistics statistics)
        {
            statistics.Force += forceEffect;
            statistics.Health += healthEffect;
            statistics.Experience += experienceEffect;
        }
        
        public void Remove(CharacterStatistics statistics)
        {
            statistics.Force -= forceEffect;
            statistics.Health -= healthEffect;
            statistics.Experience -= experienceEffect;
        }
    }
}