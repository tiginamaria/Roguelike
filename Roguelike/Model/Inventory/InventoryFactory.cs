using System;

namespace Roguelike.Model.Inventory
{
    /// <summary>
    /// A logic for inventory items creation.
    /// </summary>
    public class InventoryFactory
    {
        public InventoryItem Create(string type, Position position)
        {
            return type switch
            {
                InventoryType.IncreaseHealthItem => new IncreaseHealthItem(position, 0, 3, 0),
                InventoryType.IncreaseForceItem => new IncreaseForceItem(position, 3, 0, 0),
                InventoryType.IncreaseExperienceItem => new IncreaseExperienceItem(position, 0, 0, 3),
                InventoryType.IncreaseAllItem => new IncreaseAllItem(position, 3, 3, 3),
                _ => throw new NotSupportedException()
            };
        }
    }
}