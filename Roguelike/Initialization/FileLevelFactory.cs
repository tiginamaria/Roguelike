using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Roguelike.Model;
using Roguelike.Model.Inventory;
using Roguelike.Model.Mobs;
using Roguelike.Model.Objects;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Initialization
{
    /// <inheritdoc />
    /// <summary>
    ///     Creates a level by the given file.
    ///     The first line must contain two integers: height and width.
    ///     The next height lines contains width objects separated with spaces:
    ///     # -- wall
    ///     . -- empty
    ///     Next lines contain information about game objects:
    ///         1. Game object symbol:
    ///             $ -- player start position
    ///             * -- aggressive mob
    ///             @ -- passive mob
    ///             % -- coward mob
    ///             ? -- confused player
    ///             o -- confused mob
    ///             H -- health increase inventory 
    ///             F -- force increase inventory
    ///             E -- experience increase inventory
    ///             A -- all statistics increase inventory
    ///         2. Position (Y, X)
    ///         3*. Statistics (Experience, Force, Health) -- only for player and mob
    ///         4*. (K, N) - count of (Inventory, Applied Inventory) -- only for player
    ///         5*. List of K Inventory and N Applied Inventory then in format down -- only for player
    ///             4.1 Inventory symbol
    ///                 H -- health increase inventory 
    ///                 F -- force increase inventory
    ///                 E -- experience increase inventory
    ///                 A -- all statistics increase inventory
    ///             4.2 Inventory position (Y, X)
    /// </summary>
    public class FileLevelFactory : ILevelFactory
    {
        
        private readonly string pathToFile;

        /// <summary>
        ///     Creates the factory by the path to a file with a level description.
        /// </summary>
        public FileLevelFactory(string path)
        {
            pathToFile = path;
        }

        public override Level CreateLevel()
        {
            var lines = File.ReadAllLines(pathToFile);
            var dimensions = lines[0]
                .Split()
                .Select(int.Parse)
                .ToArray();
            var height = dimensions[0];
            var width = dimensions[1];

            lines = lines.Skip(1).ToArray();
            var boardTable = new GameObject[height, width];
            return new Level(level =>
            {
                for (var row = 0; row < height; row++)
                {
                    var inputRow = lines[row].Split().ToArray();
                    for (var col = 0; col < width; col++)
                    {
                        var gameObject = GetBoardObject(inputRow[col], new Position(row, col));
                        boardTable[row, col] = gameObject;
                    }
                }
                foreach (var s in lines.Skip(height))
                {
                    var info = s.Split();
                    var gameObject = GetGameObject(level, info);
                    boardTable[gameObject.Position.Y, gameObject.Position.X] = gameObject;
                }

                return new Board(width, height, boardTable);
            });
        }

        private static GameObject GetBoardObject(string type, Position position)
        {
            switch (type)
            {
                case BoardObject.Wall:
                    return new Wall(position);
                case BoardObject.Empty:
                    return new EmptyCell(position);
                default:
                    throw new ArgumentException($"Unknown character: {type}.");
            }
        }

        private static GameObject GetGameObject(Level level, string[] info)
        {
            var type = info[0];
            if (InventoryType.Contains(type))
            {
                return InventoryFactory.Create(type, GetPosition(1, info));
            }
            if (MobType.Contains(type))
            {
                return MobFactory.Create(type, level, 
                    GetPosition(1, info), 
                    GetStatistics(3, info));
            }
            if (PlayerType.Contains(type))
            {
                return PlayerFactory.Create(type, level, 
                    GetPosition(1, info), 
                    GetStatistics(3, info),
                    GetAllInventory(8, int.Parse(info[6]), info),
                    GetAllInventory(8 + int.Parse(info[7]) * 3, int.Parse(info[7]), info));
            }
            throw new ArgumentException($"Unknown character: {type}.");
        }
        
        private static Position GetPosition(int index, string[] info)
        {
            return new Position(int.Parse(info[index]), int.Parse(info[index + 1]));
        }
        private static CharacterStatistics GetStatistics(int index, IReadOnlyList<string> info)
        {
            return new CharacterStatistics(int.Parse(info[index]), int.Parse(info[index + 1]), int.Parse(info[index + 2]));
        }
        private static InventoryItem GetInventory(int index, string[] info)
        {
            var type = info[index];
            if (InventoryType.Contains(type))
            {
                return InventoryFactory.Create(type, GetPosition(index + 1, info));
            }
            throw new ArgumentException($"Unknown inventory type: {type}.");
        }
        private static List<InventoryItem> GetAllInventory(int index, int count, string[] info)
        {
            var inventory = new List<InventoryItem>();
            for (int i = index; i < index + count; i += 3)
            {
                inventory.Add(GetInventory(i, info));
            }

            return inventory;
        }
    }
}