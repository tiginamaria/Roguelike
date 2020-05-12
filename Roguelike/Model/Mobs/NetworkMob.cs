namespace Roguelike.Model.Mobs
{
    public class NetworkMob : Mob
    {
        public NetworkMob(Level level, IMobBehaviour behaviour, Position startPosition, 
            CharacterStatistics statistics, bool confused = false) : 
            base(level, behaviour, startPosition, statistics, confused)
        {
        }

        protected override void BecomeConfused()
        {
            Behaviour = new ConfusedMobBehaviour();
        }
    }
}