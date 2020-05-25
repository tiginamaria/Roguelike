using System;
using Roguelike.Model;
using Roguelike.Model.Objects;
using Roguelike.Model.PlayerModel;

namespace Roguelike.View
{
    /// <summary>
    /// A class for displaying the play state using the console.
    /// </summary>
    public class ConsolePlayView : IPlayView
    {
        private FixedBoundRectangle focusRectangle;

        public ConsolePlayView()
        {
            Console.CursorVisible = false;
            Console.Clear();
        }

        /// <summary>
        /// Displays the given level by drawing its board in the console.
        /// </summary>
        public void Draw(Level level)
        {
            DrawBoard(level.Board, level.CurrentPlayer);
            UpdateInventory(level);
        }

        public void UpdatePosition(Level level, Position position)
        {
            RedrawPosition(level, position);
            UpdateInventory(level);
        }

        public void UpdateMob(Level level, Position first, Position second)
        {
            RedrawPosition(level, first);
            RedrawPosition(level, second);
            UpdateInventory(level);
        }

        public void UpdatePlayer(Level level, Position first, Position second)
        {
            RedrawPlayerPosition(level, first);
            RedrawPlayerPosition(level, second);
            UpdateInventory(level);
        }

        public void UpdateInventory(Level level)
        {
            var board = level.Board;
            var statisticsRow =  Math.Min(board.Height + 2, GetConsoleBoardHeight);
            var inventoryRow =  Math.Min(board.Height + 3, GetConsoleBoardHeight + 1);
            var appliedInventoryRow =  Math.Min(board.Height + 4, GetConsoleBoardHeight + 2);
            DrawPlayerStatistics(level.CurrentPlayer, statisticsRow);
            DrawInventory(level.CurrentPlayer, inventoryRow);
            DrawAppliedInventory(level.CurrentPlayer, appliedInventoryRow);
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
                DrawBoard(level.Board, level.CurrentPlayer);
                UpdateInventory(level);
            }
        }

        private void RedrawPosition(Level level, Position position)
        {
            var consolePosition = BoardToConsolePosition(position);
            if (InsideConsole(consolePosition))
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    DrawLine(level.Board, position);
                }
                else
                {
                    DrawObject(level.Board, position, consolePosition);
                }
            }
        }

        private bool InsideConsole(Position position)
        {
            return position.X >= 0 && position.X < GetConsoleBoardWidth &&
                   position.Y >= 0 && position.Y < GetConsoleBoardHeight;
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
                            Math.Min(board.Width, GetConsoleBoardWidth),
                            Math.Min(board.Height, GetConsoleBoardHeight));

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
                Console.CursorVisible = false;
                var objectChar = board.GetObject(boardPosition).GetStringType();
                if (objectChar == ".")
                {
                    objectChar = " ";
                }
                Console.Write(objectChar);
            }
        }
                
        private void DrawPlayerStatistics(Character player, int row)
        {
            ClearRow(row);
            var statistics = player.GetStatistics();
            Console.SetCursorPosition(0, row);
            Console.Write($"Health:{statistics.Health}   Force:{statistics.Force}    Exp:{statistics.Experience}");
        }
        
        private void ClearRow(int row)
        {
            Console.SetCursorPosition(0, row);
            for (var i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write(" ");
            }
        }
        
        private void DrawInventory(AbstractPlayer player, int row)
        {
            ClearRow(row);
            var inventory = player.GetInventory();
            Console.SetCursorPosition(0, row);
            Console.Write("Inventory: ");
            foreach (var item in inventory)
            {
                Console.Write(item.GetStringType() + " ");
            }
            Console.Write("   ");
        }
        
        private void DrawAppliedInventory(AbstractPlayer player, int row)
        {
            ClearRow(row);
            var inventory = player.GetAppliedInventory();
            Console.SetCursorPosition(0, row);
            Console.Write("Applied Inventory: ");
            foreach (var item in inventory)
            {
                Console.Write(item.GetStringType() + " ");
            }
            Console.Write("   ");
        }

        private int GetConsoleBoardHeight => Console.WindowHeight - 4;
        private int GetConsoleBoardWidth => Console.WindowWidth;
    }
}
