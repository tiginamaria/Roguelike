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
                // Strange bug:
                // Windows console clears the position right to the player 
                // on attempts to redraw too frequently.
                // Decided to redraw the whole line to the right of the player.
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    DrawLine(level.Board, position);
                }
                else
                {
                    DrawObject(level.Board, position, consolePosition);
                }
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

        private void DrawLine(Board board, Position position)
        {
            for (int col = position.X; col < focusRectangle.Right; col++)
            {
                var currentPosition = new Position(position.Y, col);
                DrawObject(board, currentPosition, BoardToConsolePosition(currentPosition));
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
            if (InsideConsole(consolePosition))
            {
                Console.SetCursorPosition(consolePosition.X, consolePosition.Y);
                var objectChar = GetObjectChar(board, boardPosition);
                Console.Write(objectChar);
            }
        }

        public static char GetObjectChar(Board board, Position position)
        {
            switch (board.GetObject(position))
            {
                case EmptyCell _:
                    return EmptyChar;

                case Wall _:
                    return WallChar;

                case Player _:
                    return PlayerChar;

                case ConfusedPlayer _:
                    return ConfusedPlayerChar;

                case Mob mob:
                    return GetBehaviourChar(mob);
                
                default:
                    throw new Exception($"Invalid object found on position: {position}");
            }
        }

        private static char GetBehaviourChar(Mob mob)
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
