using System;
using Roguelike.Model;
using Roguelike.Model.Mobs;
using Roguelike.Model.Objects;
using Roguelike.Model.PlayerModel;

namespace Roguelike.View
{
    /// <summary>
    /// A class for displaying the play state using the console.
    /// </summary>
    public class ConsolePlayView : IPlayView
    {
        private const char WallChar = '#';
        private const char EmptyChar = ' ';
        private const char PlayerChar = '$';
        private const char AggressiveMobChar = '*';
        private const char PassiveMobChar = '@';
        private const char CowardMobChar = '%';
        private const char ConfusedPlayerChar = '?';
        private const char ConfusedMobChar = 'o';

        private FixedBoundRectangle focusRectangle;

        public ConsolePlayView()
        {
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight);
            Console.CursorVisible = false;
        }
        
        /// <summary>
        /// Displays the given level by drawing its board in the console.
        /// </summary>
        public void Draw(Level level) => DrawBoard(level.Board, level.Player);

        public void UpdateMob(Level level, Position first, Position second)
        {
            RedrawPosition(level, first);
            RedrawPosition(level, second);
        }

        public void UpdatePlayer(Level level, Position first, Position second)
        {
            RedrawPlayerPosition(level, first);
            RedrawPlayerPosition(level, second);
        }
        
        private void RedrawPlayerPosition(Level level, Position position)
        {
            var consolePosition = BoardToConsolePosition(position);
            if (InsideConsole(consolePosition))
            {
                DrawObject(level.Board, position, consolePosition);
            }
            else
            {
                DrawBoard(level.Board, level.Player);
            }
        }

        private void RedrawPosition(Level level, Position position)
        {
            var consolePosition = BoardToConsolePosition(position);
            if (InsideConsole(consolePosition))
            {
                DrawObject(level.Board, position, consolePosition);
            }
        }

        private bool InsideConsole(Position position)
        {
            return position.X >= 0 && position.X < Console.WindowWidth &&
                   position.Y >= 0 && position.Y < Console.WindowHeight;
        }

        private void DrawBoard(Board board, GameObject focus)
        {
            focusRectangle = GetFocusRectangle(board, focus);
            for (var row = focusRectangle.Top; row < focusRectangle.Bottom; row++)
            {
                for (var col = focusRectangle.Left; col < focusRectangle.Right; col++)
                {
                    var currentPosition = new Position(row, col);
                    DrawObject(board, currentPosition, BoardToConsolePosition(currentPosition));
                }
            }
        }

        private Position BoardToConsolePosition(Position boardPosition) => 
            new Position(boardPosition.Y - focusRectangle.Top, boardPosition.X - focusRectangle.Left);

        /// <summary>
        /// Returns a rectangle focused on the given Game Object,
        /// so that its centre equals to the object position if possible.
        /// </summary>
        private FixedBoundRectangle GetFocusRectangle(Board board, GameObject focus)
        {
            var newRectangle = new FixedBoundRectangle(
                            0,
                            0,
                            Math.Min(board.Width, Console.WindowWidth),
                            Math.Min(board.Height, Console.WindowHeight));

            var playerPosition = focus.Position;

            if (playerPosition.X - newRectangle.Width / 2 >= 0)
            {
                newRectangle.Right = Math.Min(board.Width, playerPosition.X + newRectangle.Width / 2);
            }
            
            if (playerPosition.X + newRectangle.Width / 2 < board.Width)
            {
                newRectangle.Left = Math.Max(0, playerPosition.X - newRectangle.Width / 2);
            }

            if (playerPosition.Y - newRectangle.Height / 2 >= 0)
            {
                newRectangle.Bottom = Math.Min(board.Height, playerPosition.Y + newRectangle.Height / 2);
            }

            if (playerPosition.Y + newRectangle.Height / 2 < board.Height)
            {
                newRectangle.Top = Math.Max(0, playerPosition.Y - newRectangle.Height / 2);
            }
            return newRectangle;
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

            if (gameObject is ConfusedPlayer)
            {
                return ConfusedPlayerChar;
            }
            
            if (gameObject is Mob)
            {
                return GetBehaviourChar(gameObject as Mob);
            }

            throw new Exception($"Invalid object found: {gameObject}");
        }

        private char GetBehaviourChar(Mob mob)
        {
            var behaviour = mob.Behaviour;
            if (behaviour is AggressiveMobBehaviour)
            {
                return AggressiveMobChar;
            }
            if (behaviour is PassiveMobBehaviour)
            {
                return PassiveMobChar;
            }

            if (behaviour is CowardMobBehaviour)
            {
                return CowardMobChar;
            }

            if (behaviour is ConfusedMobBehaviour)
            {
                return ConfusedMobChar;
            }

            throw new Exception($"Invalid behaviour found: {behaviour}");
        }
    }
}
