namespace Roguelike.Model
{
    public class CowardMobBehaviour : IMobBehaviour
    {
        public Position MakeMove(Level level, Position position)
        {
            var newPosition = level.Graph.Farthest(position, level.Player.Position);
            level.Board.MoveObject(position, newPosition);
            return newPosition;
        }
    }
}