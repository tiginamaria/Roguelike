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
        private Random Random = new Random();

        public InventoryItem Create(string type, Position position)
        {
            switch (type)
            {
                case InventoryType.IncreaseHealthItem:
                    return new IncreaseHealthItem(position, 0, Random.Next(1, 6), 0);
                case InventoryType.IncreaseForceItem:
                    return new IncreaseForceItem(position, Random.Next(1, 6), 0, 0);
                case InventoryType.IncreaseExperienceItem:
                    return new IncreaseExperienceItem(position, 0, 0, Random.Next(1, 6));
                case InventoryType.IncreaseAllItem:
                    return new IncreaseAllItem(position, Random.Next(1, 6), Random.Next(1, 6), Random.Next(1, 6));
                default:
                    throw new NotSupportedException();
            }
        }
    }
}