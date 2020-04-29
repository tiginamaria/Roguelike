namespace Roguelike.Model.Mobs
{
    public class CowardMobBehaviour : IMobBehaviour
    {
        private const int SearchRadius = 10;
        
        public Position MakeMove(Level level, Position position)
        {
            var graph = level.Graph;
            if (graph.GetDistance(position, level.Player.Position) <= SearchRadius)
            {
                return level.Graph.Farthest(position, level.Player.Position);
            }

            return position;
        }

        public string GetStringType()
        {
            return MobType.CowardMob;
        }
    }
}
