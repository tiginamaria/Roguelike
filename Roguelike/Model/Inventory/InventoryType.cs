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
            return new[]
            {
                IncreaseHealthItem,
                IncreaseForceItem,
                IncreaseExperienceItem,
                IncreaseAllItem
            }.Contains(symbol);
        }
    }
}