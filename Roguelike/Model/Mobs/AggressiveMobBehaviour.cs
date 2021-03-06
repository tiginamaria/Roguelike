namespace Roguelike.Model.Mobs
{
    /// <summary>
    /// A behaviour when a mob follows the nearest player in some radius.
    /// </summary>
    public class AggressiveMobBehaviour : IMobBehaviour
    {
        private const int SearchRadius = 3;

        public Position MakeMove(Level level, Position position)
        {
            var graph = level.Graph;
            var nearestPlayerPosition = level.NearestPlayerPosition(position);
            if (nearestPlayerPosition != null)
            {
                var notNullNearestPlayerPosition = (Position) nearestPlayerPosition;
                if (graph.GetDistance(position, notNullNearestPlayerPosition) <= SearchRadius)
                {
                    return level.Graph.Nearest(position, notNullNearestPlayerPosition);
                }
            }

            return position;
        }

        public string GetStringType() => MobType.AggressiveMob;
    }
}