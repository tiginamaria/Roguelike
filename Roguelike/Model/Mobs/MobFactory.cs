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
        public Mob Create(string type, Level level, Position position)
        {
            return Create(type, level, position, new CharacterStatistics(2, 3, 1));
        }

        public Mob Create(string type, Level level, Position position, CharacterStatistics statistics)
        {
            return type switch
            {
                MobType.AggressiveMob => new Mob(level, new AggressiveMobBehaviour(), position, statistics),
                MobType.CowardMob => new Mob(level, new CowardMobBehaviour(), position, statistics),
                MobType.PassiveMob => new Mob(level, new PassiveMobBehaviour(), position, statistics),
                MobType.ConfusedMob => new Mob(level, new PassiveMobBehaviour(), position, statistics, true),
                _ => throw new NotSupportedException()
            };
        }
    }
}