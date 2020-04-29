namespace Roguelike.Model.Inventory
{
    public class IncreaseAllItem : InventoryItem
    {
        public IncreaseAllItem(Position position, int forceEffect, int healthEffect, int experienceEffect) : base(position, forceEffect, healthEffect, experienceEffect)
        {
        }

        public override string GetStringType()
        {
            return InventoryType.IncreaseAllItem;
        }
    }
}