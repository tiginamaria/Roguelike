namespace Roguelike.Model.Inventory
{
    /// <summary>
    /// Increases all player's characteristics.
    /// </summary>
    public class IncreaseAllItem : InventoryItem
    {
        public IncreaseAllItem(Position position, int forceEffect, int healthEffect, int experienceEffect) : 
            base(position, forceEffect, healthEffect, experienceEffect)
        {
        }

        public override string GetStringType() => InventoryType.IncreaseAllItem;
    }
}