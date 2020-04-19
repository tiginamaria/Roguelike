using System;

namespace Roguelike.Model
{
    public class PassiveMobBehaviour : IMobBehaviour
    {
        private static readonly int[] Dx = {0, 0, -1, 1};
        private static readonly int[] Dy = {-1, 1, 0, 0};
        private static readonly Random Random = new Random();

        public Position MakeMove(Level level, Position position)
        {
            var i = Random.Next(4);
            var nx = position.X + Dx[i];
            var ny = position.Y + Dy[i];
            var np = new Position(ny, nx);
            if (!level.Board.CheckOnBoard(position) || !level.Board.IsEmpty(np)) return position;
            level.Board.MoveObject(position, np);
            return np;
        }
    }
}