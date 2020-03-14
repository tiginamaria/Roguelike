using System;

namespace Roguelike
{
    public class PlayView
    {
        private const char Wall = '#';
        private const char Empty = '.';
        private const char Player = '$';
        
        public void Draw(Level level)
        {
            DrawBoard(level.Board);
        }

        private void DrawBoard(Board board)
        {
            var width = board.Width;
            var height = board.Height;

            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    DrawObject(board, new Position(row, col));
                }
            }
        }

        private void DrawObject(Board board, Position position)
        {
            Console.SetCursorPosition(position.X, position.Y);
            var objectChar = GetObjectChar(board, position);
            Console.Write(objectChar);
        }

        private char GetObjectChar(Board board, Position position)
        {
            if (board.IsWall(position))
            {
                return Wall;
            }

            if (board.IsEmpty(position))
            {
                return Empty;
            }

            var gameObject = board.GetObject(position);
            if (gameObject is Player)
            {
                return Player;
            }
            
            throw new Exception($"Invalid object found: {gameObject}");
        }
    }
}