using System;
using System.Linq;

namespace Roguelike.Model.Mobs
{
    public class MobType
    {
        public const string AggressiveMob = "*";
        public const string PassiveMob = "@";
        public const string CowardMob = "%";
        public const string ConfusedMob = "o";

        public static bool Contains(string type)
        {
            return new[] {AggressiveMob, PassiveMob, CowardMob, ConfusedMob}.Contains(type);
        }
    }
    
    public class MobFactory
    {
        public Mob Create(string type, Level level, Position position, CharacterStatistics statistics) {
            switch (type) {
                case MobType.AggressiveMob:
                    return new Mob(level, new AggressiveMobBehaviour(), position, statistics);
                case MobType.CowardMob:
                    return new Mob(level, new CowardMobBehaviour(), position, statistics);
                case MobType.PassiveMob:
                    return new Mob(level, new PassiveMobBehaviour(), position, statistics);
                case MobType.ConfusedMob:
                    return new Mob(level, new PassiveMobBehaviour(), position, statistics, true);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}