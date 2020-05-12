using System;

namespace Roguelike.Model.Mobs
{
    public class NetworkMobFactory : MobFactory
    {
        public override Mob Create(string type, Level level, Position position, CharacterStatistics statistics)
        {
            return type switch
            {
                MobType.AggressiveMob => new NetworkMob(level, new AggressiveMobBehaviour(), position, statistics),
                MobType.CowardMob => new NetworkMob(level, new CowardMobBehaviour(), position, statistics),
                MobType.PassiveMob => new NetworkMob(level, new PassiveMobBehaviour(), position, statistics),
                MobType.ConfusedMob => new NetworkMob(level, new PassiveMobBehaviour(), position, statistics, true),
                _ => throw new NotSupportedException()
            };
        }
    }
}