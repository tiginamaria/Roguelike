namespace Roguelike.Model
{
    public class CowardMobBehaviour : IMobBehaviour
    {
        public Position MakeMove(Level level, Position position)
        {
            return level.Graph.Farthest(position, level.Player.Position);
        }
    }
}