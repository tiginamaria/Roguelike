namespace Roguelike.Model.Mobs
{
    public class PassiveMobBehaviour : IMobBehaviour
    {
        public Position MakeMove(Level level, Position position) => position;

        public string GetStringType() => MobType.PassiveMob;
    }
}
