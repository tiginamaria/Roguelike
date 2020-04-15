using System;
using Roguelike.Model;

namespace Roguelike.View
{
    /// <summary>
    /// A class for displaying the play state using the console.
    /// </summary>
    public class ConsolePlayView : IPlayView
    {
        private const char WallChar = '#';
        private const char EmptyChar = '.';
        private const char PlayerChar = '$';

        public ConsolePlayView()
        {
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight);
        }
        
        /// <summary>
        /// Displays the given level by drawing its board in the console.
        /// </summary>
        public void Draw(Level level) => DrawBoard(level.Board, level.Player);

        private void DrawBoard(Board board, GameObject focus)
        {
            var focusRectangle = GetFocusRectangle(board, focus);
            for (var row = focusRectangle.Top; row < focusRectangle.Bottom; row++)
            {
                for (var col = focusRectangle.Left; col < focusRectangle.Right; col++)
                {
                    DrawObject(board, 
                        new Position(row, col), 
                        new Position(row - focusRectangle.Top, col - focusRectangle.Left));
                }
            }
        }

        /// <summary>
        /// Returns a rectangle focused on the given Game Object,
        /// so that its centre equals to the object position if possible.
        /// </summary>
        private FixedBoundRectangle GetFocusRectangle(Board board, GameObject focus)
        {
            var focusRectangle = new FixedBoundRectangle(
                            0,
                            0,
                            Math.Min(board.Width, Console.WindowWidth),
                            Math.Min(board.Height, Console.WindowHeight));

            var playerPosition = focus.Position;

            if (playerPosition.X - focusRectangle.Width / 2 >= 0)
            {
                focusRectangle.Right = Math.Min(board.Width, playerPosition.X + focusRectangle.Width / 2);
            }
            
            if (playerPosition.X + focusRectangle.Width / 2 < board.Width)
            {
                focusRectangle.Left = Math.Max(0, playerPosition.X - focusRectangle.Width / 2);
            }

            if (playerPosition.Y - focusRectangle.Height / 2 >= 0)
            {
                focusRectangle.Bottom = Math.Min(board.Height, playerPosition.Y + focusRectangle.Height / 2);
            }

            if (playerPosition.Y + focusRectangle.Height / 2 < board.Height)
            {
                focusRectangle.Top = Math.Max(0, playerPosition.Y - focusRectangle.Height / 2);
            }
            return focusRectangle;
        }

        private void DrawObject(Board board, Position boardPosition, Position consolePosition)
        {
            Console.SetCursorPosition(consolePosition.X, consolePosition.Y);
            var objectChar = GetObjectChar(board, boardPosition);
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
