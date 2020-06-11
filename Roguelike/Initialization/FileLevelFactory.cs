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
    ///         2*. Login
    ///         3. Position (Y, X)
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
    public class FileLevelFactory : LevelFactory
    {
        private string[] lines;

        /// <summary>
        /// Creates the factory by the path to a file with a level description.
        /// </summary>
        public FileLevelFactory(string path) => lines = File.ReadAllLines(path);

        private FileLevelFactory(string[] lines) => this.lines = lines;

        public static FileLevelFactory FromString(string snapshot) =>
            new FileLevelFactory(snapshot
                .Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries)
                .ToArray());

        public override Level CreateLevel()
        {
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
                    var inputRow = lines[row].Trim().Split();
                    for (var col = 0; col < width; col++)
                    {
                        var gameObject = GetBoardObject(inputRow[col], new Position(row, col));
                        boardTable[row, col] = gameObject;
                    }
                }

                foreach (var s in lines.Skip(height))
                {
                    var info = s.Trim().Split();
                    var gameObject = GetGameObject(level, info);
                    boardTable[gameObject.Position.Y, gameObject.Position.X] = gameObject;
                }
                
                return new Board(width, height, boardTable);
            });
        }

        private static GameObject GetBoardObject(string type, Position position)
        {
            return type switch
            {
                BoardObject.Wall => new Wall(position),
                BoardObject.Empty => new EmptyCell(position),
                _ => throw new ArgumentException($"Unknown character: {type}.")
            };
        }

        private GameObject GetGameObject(Level level, string[] info)
        {
            var index = 0;
            var type = info[index++];
            if (InventoryType.Contains(type))
            {
                return InventoryFactory.Create(type, GetPosition(ref index, info));
            }

            if (MobType.Contains(type))
            {
                return MobFactory.Create(type, level, 
                    GetPosition(ref index, info), 
                    GetStatistics(ref index, info));
            }

            if (PlayerType.Contains(type))
            {
                return PlayerFactory.Create(
                    GetLogin(ref index, info), type, level, 
                    GetPosition(ref index, info), 
                    GetStatistics(ref index, info),
                    GetAllInventory(ref index, info),
                    GetAllInventory(ref index, info));
            }

            throw new ArgumentException($"Unknown character: {type}.");
        }

        private static string GetLogin(ref int index, string[] info)
        {
            return info[index++];
        }

        private static Position GetPosition(ref int index, string[] info) => 
            new Position(int.Parse(info[index++]), int.Parse(info[index++]));

        private static CharacterStatistics GetStatistics(ref int index, IReadOnlyList<string> info) =>
            new CharacterStatistics(int.Parse(info[index++]), 
                int.Parse(info[index++]), int.Parse(info[index++]));

        private InventoryItem GetInventory(ref int index, string[] info)
        {
            var type = info[index++];
            if (InventoryType.Contains(type))
            {
                return InventoryFactory.Create(type, GetPosition(ref index, info));
            }

            throw new ArgumentException($"Unknown inventory type: {type}.");
        }

        private List<InventoryItem> GetAllInventory(ref int index, string[] info)
        {
            var inventory = new List<InventoryItem>();
            var count = int.Parse(info[index++]);
            while (inventory.Count < count)
            {
                inventory.Add(GetInventory(ref index, info));
            }

            return inventory;
        }
    }
}