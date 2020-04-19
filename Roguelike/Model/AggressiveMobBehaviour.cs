namespace Roguelike.Model
{
    public class AggressiveMobBehaviour : IMobBehaviour
    {
        public Position MakeMove(Level level, Position position)
        {
            var newPosition = level.Graph.Nearest(position, level.Player.Position);
            level.Board.MoveObject(position, newPosition);
            return newPosition;
        }
    }
}