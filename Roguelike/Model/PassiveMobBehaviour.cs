using System;

namespace Roguelike.Model
{
    public class PassiveMobBehaviour : IMobBehaviour
    {
        private static readonly int[] dx = {0, 0, -1, 1, 0};
        private static readonly int[] dy = {-1, 1, 0, 0, 0};
        private static readonly Random Random = new Random();

        public Position MakeMove(Level level, Position position)
        {
            var i = Random.Next(dx.Length);
            var newX = position.X + dx[i];
            var newY = position.Y + dy[i];
            var newPosition = new Position(newY, newX);
            if (!level.Board.CheckOnBoard(position) || level.Board.IsWall(newPosition))
            {
                return position;
            }
            return newPosition;
        }
    }
}