using System;

namespace Roguelike
{
    public class RandomLevelFactory : ILevelFactory
    {
        private const int DefaultHeight = 15;
        private const int DefaultWidth = 15;
        private readonly Random random = new Random();
        private const float WallProbability = 0.5f;

        private static readonly int Height = RoundToOdd(DefaultHeight);
        private static readonly int Width = RoundToOdd(DefaultWidth);
        private static readonly int CellHeight = (Height - 1) / 2;
        private static readonly int CellWidth = (Width - 1) / 2;
        private readonly Labyrinth labyrinth = new Labyrinth(CellHeight, CellWidth);

        private static int RoundToOdd(int x)
        {
            if (x % 2 == 0)
            {
                return x + 1;
            }
            else
            {
                return x;
            }
        }

        public Level CreateLevel()
        {
            var boardTable = new GameObject[Height, Width];
            CreateBorders(Height, Width, boardTable);
            var dx = new[] {0, 1};
            var dy = new[] {1, 0};
            for (var cellRow = 0; cellRow < CellHeight; cellRow++)
            {
                for (var cellCol = 0; cellCol < CellWidth; cellCol++)
                {
                    var row = CellIdToBoard(cellRow);
                    var col = CellIdToBoard(cellCol);
                    AddEmptyCell(row, col, boardTable);
                    for (var i = 0; i < 2; ++i)
                    {
                        if (!labyrinth.IsValidCell(cellRow + dy[i], cellCol + dx[i])) continue;
                        ConnectCells(cellRow, cellCol, cellRow + dy[i], cellCol + dx[i], boardTable);
                    }

                    if (labyrinth.IsValidCell(cellRow + 1, cellCol + 1))
                    {
                        AddWallWithProbability(row + 1, col + 1, boardTable);
                    }
                }
            }

            AddPlayerCell(1, 1, boardTable);

            var board = new Board(Width, Height, boardTable);
            return new Level(board);
        }

        private void ConnectCells(int cellY1, int cellX1, int cellY2, int cellX2, GameObject[,] boardTable)
        {
            var row = CellIdToBoard(cellY1) + cellY2 - cellY1;
            var col = CellIdToBoard(cellX1) + cellX2 - cellX1;
            if (labyrinth.AreConnected(cellY1, cellX1, cellY2, cellX2))
            {
                AddEmptyCell(row, col, boardTable);
            }
            else
            {
                AddWallWithProbability(row, col, boardTable);
            }
        }

        private static int CellIdToBoard(int id)
        {
            return 1 + id * 2;
        }

        private static void CreateBorders(int height, int width, GameObject[,] boardTable)
        {
            var rows = new[] {0, height - 1};
            foreach (var row in rows)
            {
                for (var col = 0; col < width; ++col)
                {
                    AddWall(row, col, boardTable);
                }
            }

            var cols = new[] {0, width - 1};
            foreach (var col in cols)
            {
                for (var row = 0; row < height; ++row)
                {
                    AddWall(row, col, boardTable);
                }
            }
        }

        private static void AddWall(int row, int col, GameObject[,] boardTable)
        {
            boardTable[row, col] = new Wall(new Position(row, col));
        }

        private void AddWallWithProbability(int row, int col, GameObject[,] boardTable)
        {
            if (random.NextDouble() < WallProbability)
            {
                AddWall(row, col, boardTable);
            }
            else
            {
                AddEmptyCell(row, col, boardTable);
            }
        }

        private static void AddEmptyCell(int row, int col, GameObject[,] boardTable)
        {
            boardTable[row, col] = new EmptyCell(new Position(row, col));
        }

        private static void AddPlayerCell(int row, int col, GameObject[,] boardTable)
        {
            boardTable[row, col] = new Player(new Position(row, col));
        }
    }
}