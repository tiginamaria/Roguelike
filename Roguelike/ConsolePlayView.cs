using System;

namespace Roguelike
{
    public class ConsolePlayView
    {
        private const char WallChar = '#';
        private const char EmptyChar = '.';
        private const char PlayerChar = '$';

        private Rectangle bufferRectangle;

        public ConsolePlayView()
        {
            bufferRectangle = new Rectangle(
                left: 0,
                right: Console.WindowWidth,
                top: 0,
                bottom: Console.WindowHeight);
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight);
        }
        
        public void Draw(Level level)
        {
            DrawBoard(level.Board, level.Player);
        }

        private void DrawBoard(Board board, Player player)
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
                return WallChar;
            }

            if (board.IsEmpty(position))
            {
                return EmptyChar;
            }

            var gameObject = board.GetObject(position);
            if (gameObject is Player)
            {
                return PlayerChar;
            }
            
            throw new Exception($"Invalid object found: {gameObject}");
        }
    }
}