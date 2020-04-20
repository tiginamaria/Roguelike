namespace Roguelike.Model.Mobs
{
    public class AggressiveMobBehaviour : IMobBehaviour
    {
        public Position MakeMove(Level level, Position position)
        {
            return level.Graph.Nearest(position, level.Player.Position);
        }
    }
}
