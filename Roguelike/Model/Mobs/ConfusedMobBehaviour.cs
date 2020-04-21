using System;

namespace Roguelike.Model.Mobs
{
    public class ConfusedMobBehaviour : IMobBehaviour
    {
        private static readonly int[] Dx = {0, 0, -1, 1, 0};
        private static readonly int[] Dy = {-1, 1, 0, 0, 0};
        private static readonly Random Random = new Random();

        public Position MakeMove(Level level, Position position)
        {
            var i = Random.Next(Dx.Length);
            var newX = position.X + Dx[i];
            var newY = position.Y + Dy[i];
            var newPosition = new Position(newY, newX);
            if (!level.Board.CheckOnBoard(newPosition) || level.Board.IsWall(newPosition))
            {
                return position;
            }
            return newPosition;
        }
    }
}