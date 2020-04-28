using System;
using System.Linq;

namespace Roguelike.Model.Inventory
{
    public class InventoryType
    {
        public const string IncreaseHealthItem = "H";
        public const string IncreaseForceItem = "F";
        public const string IncreaseExperienceItem = "E";
        public const string IncreaseAllItem = "A";

        public static bool Contains(string symbol)
        {
            return new[] {IncreaseHealthItem, IncreaseForceItem, IncreaseExperienceItem, IncreaseAllItem}.Contains(symbol);
        }
    }

    public class InventoryFactory
    {
        public InventoryItem Create(string type, Position position)
        {
            switch (type)
            {
                case InventoryType.IncreaseHealthItem:
                    return new IncreaseHealthItem(position, 0, 3, 0);
                case InventoryType.IncreaseForceItem:
                    return new IncreaseForceItem(position, 3, 0, 0);
                case InventoryType.IncreaseExperienceItem:
                    return new IncreaseExperienceItem(position, 0, 0, 3);
                case InventoryType.IncreaseAllItem:
                    return new IncreaseAllItem(position, 3, 3, 3);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}