namespace Roguelike.Model.Mobs
{
    public class AggressiveMobBehaviour : IMobBehaviour
    {
        private const int SearchRadius = 10;
        
        public Position MakeMove(Level level, Position position)
        {
            var graph = level.Graph;
            if (graph.GetDistance(position, level.Player.Position) <= SearchRadius)
            {
                return level.Graph.Nearest(position, level.Player.Position);
            }

            return position;
        }
    }
}
