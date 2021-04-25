namespace Roguelike.Model.Mobs
{
    /// <summary>
    /// A mob behaviour when it does not move.
    /// </summary>
    public class PassiveMobBehaviour : IMobBehaviour
    {
        public Position MakeMove(Level level, Position position) => position;

        public string GetStringType() => MobType.PassiveMob;
    }
}
