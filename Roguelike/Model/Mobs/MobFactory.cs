using System;

namespace Roguelike.Model.Mobs
{
    public class MobFactory
    {
        public Mob Create(string type, Level level, Position position) => 
            Create(type, level, position, new CharacterStatistics(2, 3, 1));

        public virtual Mob Create(string type, Level level, Position position, CharacterStatistics statistics)
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