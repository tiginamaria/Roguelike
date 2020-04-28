namespace Roguelike.Model.Mobs
{
    public class PassiveMobBehaviour : IMobBehaviour
    {
        public Position MakeMove(Level level, Position position)
        {
            return position;
        }

        public string GetType()
        {
            return MobType.PassiveMob;
        }
    }
}
