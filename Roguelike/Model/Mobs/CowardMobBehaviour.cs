namespace Roguelike.Model.Mobs
{
    public class CowardMobBehaviour : IMobBehaviour
    {
        private const int SearchRadius = 10;

        public Position MakeMove(Level level, Position position)
        {
            var graph = level.Graph;
            var nearestPlayerPosition = level.NearestPlayerPosition(position);
            if (nearestPlayerPosition != null)
            {
                var notNullNearestPlayerPosition = (Position) nearestPlayerPosition;
                if (graph.GetDistance(position, notNullNearestPlayerPosition) <= SearchRadius)
                {
                    return level.Graph.Farthest(position, notNullNearestPlayerPosition);
                }
            }

            return position;
        }

        public string GetStringType() => MobType.CowardMob;
    }
}