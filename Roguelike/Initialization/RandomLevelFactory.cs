using System;
using Roguelike.Model;
using Roguelike.Model.Inventory;
using Roguelike.Model.Mobs;
using Roguelike.Model.Objects;

namespace Roguelike.Initialization
{
    /// <inheritdoc />
    /// <summary>
    /// A factory to generate random levels.
    /// Default dimensions are 20 * 20.
    /// </summary>
    public class RandomLevelFactory : LevelFactory
    {
        private const int DefaultHeight = 20;
        private const int DefaultWidth = 20;
        private readonly Random random = new Random();
        private const float WallProbability = 0.5f;
        private const float MobProbability = 0.05f;
        private const float InventoryProbability = 0.03f;
        private const float MobTypeProbability = 0.33f;
        private const float InventoryTypeProbability = 0.2f;

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

            return x;
        }

        public override Level CreateLevel()
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
                    for (var i = 0; i < 2; i++)
                    {
                        if (!labyrinth.IsValidCell(cellRow + dy[i], cellCol + dx[i]))
                        {
                            continue;
                        }

                        ConnectCells(cellRow, cellCol, cellRow + dy[i], cellCol + dx[i], boardTable);
                    }

                    if (labyrinth.IsValidCell(cellRow + 1, cellCol + 1))
                    {
                        AddWallWithProbability(row + 1, col + 1, boardTable);
                    }
                }
            }

            return new Level(level =>
            {
                AddMobs(level, boardTable, Height, Width);
                AddInventory(boardTable, Height, Width);
                return new Board(Width, Height, boardTable);
            });
        }

        private void AddInventory(GameObject[,] boardTable, int height, int width)
        {
            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    if (boardTable[row, col] is EmptyCell)
                    {
                        AddInventoryWithProbability(row, col, boardTable);
                    }
                }
            }
        }

        private void AddInventoryWithProbability(int row, int col, GameObject[,] boardTable)
        {
            if (random.NextDouble() < InventoryProbability)
            {
                boardTable[row, col] = InventoryFactory.Create(GetInventoryMobType(), new Position(row, col));
            }
        }

        private string GetInventoryMobType()
        {
            var probability = random.NextDouble();
            if (probability < InventoryTypeProbability)
            {
                return InventoryType.IncreaseExperienceItem;
            }

            if (probability < 2 * InventoryTypeProbability)
            {
                return InventoryType.IncreaseHealthItem;
            }
            
            if (probability < 4 * InventoryTypeProbability)
            {
                return InventoryType.IncreaseForceItem;
            }

            return InventoryType.IncreaseAllItem;
        }
        
        private void AddMobs(Level level, GameObject[,] boardTable, int height, int width)
        {
            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    if (boardTable[row, col] is EmptyCell)
                    {
                        AddMobWithProbability(row, col, boardTable, level);
                    }
                }
            }
        }

        private void AddMobWithProbability(int row, int col, GameObject[,] boardTable, Level level)
        {
            if (random.NextDouble() < MobProbability)
            {
                boardTable[row, col] = MobFactory.Create(GetRandomMobType(), level, new Position(row, col));
            }
        }

        private string GetRandomMobType()
        {
            var probability = random.NextDouble();
            if (probability < MobTypeProbability)
            {
                return MobType.AggressiveMob;
            }

            if (probability < 2 * MobTypeProbability)
            {
                return MobType.CowardMob;
            }

            return MobType.PassiveMob;
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
                for (var col = 0; col < width; col++)
                {
                    AddWall(row, col, boardTable);
                }
            }

            var cols = new[] {0, width - 1};
            foreach (var col in cols)
            {
                for (var row = 0; row < height; row++)
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
    }
}
