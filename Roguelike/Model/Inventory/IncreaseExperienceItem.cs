namespace Roguelike.Model.Inventory
{
    public class IncreaseExperienceItem : InventoryItem
    {
        public IncreaseExperienceItem(Position position, int forceEffect, int healthEffect, int experienceEffect) : 
            base(position, forceEffect, healthEffect, experienceEffect)
        {
        }

        public override string GetStringType() => InventoryType.IncreaseExperienceItem;
    }
}