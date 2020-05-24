namespace Roguelike.Model.Mobs
{
    /// <summary>
    /// A representation of the mob in a network mode.
    /// </summary>
    public class NetworkMob : Mob
    {
        public NetworkMob(Level level, IMobBehaviour behaviour, Position startPosition, 
            CharacterStatistics statistics, bool confused = false) : 
            base(level, behaviour, startPosition, statistics, confused)
        {
        }

        protected override void BecomeConfused() => Behaviour = new ConfusedMobBehaviour();
    }
}