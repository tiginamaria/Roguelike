namespace Roguelike.Model.Inventory
{
    /// <summary>
    /// Increases player's health.
    /// </summary>
    public class IncreaseHealthItem : InventoryItem
    {
        public IncreaseHealthItem(Position position, int forceEffect, int healthEffect, int experienceEffect) : 
            base(position, forceEffect, healthEffect, experienceEffect)
        {
        }

        public override string GetStringType() => InventoryType.IncreaseHealthItem;
    }
}