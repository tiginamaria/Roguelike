namespace Roguelike.Model.Inventory
{
    public class IncreaseHealthItem : InventoryItem
    {
        public IncreaseHealthItem(Position position, int forceEffect, int healthEffect, int experienceEffect) : base(position, forceEffect, healthEffect, experienceEffect)
        {
        }
    }
}