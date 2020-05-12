using System.Linq;

namespace Roguelike.Model.Mobs
{
    public static class MobType
    {
        public const string AggressiveMob = "*";
        public const string PassiveMob = "@";
        public const string CowardMob = "%";
        public const string ConfusedMob = "o";

        public static bool Contains(string type) => 
            new[] {AggressiveMob, PassiveMob, CowardMob, ConfusedMob}.Contains(type);
    }
}