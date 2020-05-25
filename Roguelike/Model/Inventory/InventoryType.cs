using System.Linq;

namespace Roguelike.Model.Inventory
{
    /// <summary>
    /// A string representation of inventory items.
    /// </summary>
    public static class InventoryType
    {
        public const string IncreaseHealthItem = "H";
        public const string IncreaseForceItem = "F";
        public const string IncreaseExperienceItem = "E";
        public const string IncreaseAllItem = "A";

        public static bool Contains(string symbol) =>
            new[]
            {
                IncreaseHealthItem,
                IncreaseForceItem,
                IncreaseExperienceItem,
                IncreaseAllItem
            }.Contains(symbol);
    }
}