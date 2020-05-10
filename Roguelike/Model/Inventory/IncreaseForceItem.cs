namespace Roguelike.Model.Inventory
{
    public class IncreaseForceItem : InventoryItem
    {
        public IncreaseForceItem(Position position, int forceEffect, int healthEffect, int experienceEffect) : 
            base(position, forceEffect, healthEffect, experienceEffect)
        {
        }

        public override string GetStringType()
        {
            return InventoryType.IncreaseForceItem;
        }
    }
}