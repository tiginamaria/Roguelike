using System;

namespace Roguelike.Model.Inventory
{
    public abstract class InventoryItem
    {
        private int ForceEffect;
        private int HealthEffect;
        private int ExperienceEffect;

        public InventoryItem(int forceEffect, int healthEffect, int experienceEffect)
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
    
    public enum InventoryItemType
    {
        IncreaseHealth,
        IncreaseForce,
        IncreaseExperience,
        IncreaseAll
    }
    
    public class IncreaseHealthItem : InventoryItem
    {
        public IncreaseHealthItem(int forceEffect, int healthEffect, int experienceEffect) : base(forceEffect, healthEffect, experienceEffect)
        {
        }
    }
    
    public class IncreaseForceItem : InventoryItem
    {
        public IncreaseForceItem(int forceEffect, int healthEffect, int experienceEffect) : base(forceEffect, healthEffect, experienceEffect)
        {
        }
    }
    
    public class IncreaseExperienceItem : InventoryItem
    {
        public IncreaseExperienceItem(int forceEffect, int healthEffect, int experienceEffect) : base(forceEffect, healthEffect, experienceEffect)
        {
        }
    }
    
    public class IncreaseAllItem : InventoryItem
    {
        public IncreaseAllItem(int forceEffect, int healthEffect, int experienceEffect) : base(forceEffect, healthEffect, experienceEffect)
        {
        }
    }

    public class InventoryFactory
    {
        private Random Random = new Random();
        
        public InventoryItem GetInventoryItem(InventoryItemType type) {
            switch (type) {
                case InventoryItemType.IncreaseHealth:
                    return new IncreaseHealthItem(0, Random.Next(1, 6), 0);
                case InventoryItemType.IncreaseForce:
                    return new IncreaseForceItem(Random.Next(1, 6), 0, 0);
                case InventoryItemType.IncreaseExperience:
                    return new IncreaseExperienceItem(0, 0, Random.Next(1, 6));
                case InventoryItemType.IncreaseAll:
                    return new IncreaseAllItem(Random.Next(1, 6), Random.Next(1, 6), Random.Next(1, 6));
                default:
                    throw new NotSupportedException();
            }
        }
    }
}