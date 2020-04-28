using Roguelike.Model.Objects;

namespace Roguelike.Model.Inventory
{
    public abstract class InventoryItem: GameObject
    {
        private int ForceEffect;
        private int HealthEffect;
        private int ExperienceEffect;

        public InventoryItem(Position position, int forceEffect, int healthEffect, int experienceEffect) : base(position)
        {
            ForceEffect = forceEffect;
            HealthEffect = healthEffect;
            ExperienceEffect = experienceEffect;
        }

        public void Apply(CharacterStatistics statistics)
        {
            statistics.Force += ForceEffect;
            statistics.Health += HealthEffect;
            statistics.Experience += ExperienceEffect;
        }
        
        public void Remove(CharacterStatistics statistics)
        {
            statistics.Force -= ForceEffect;
            statistics.Health -= HealthEffect;
            statistics.Experience -= ExperienceEffect;
        }
    }
}