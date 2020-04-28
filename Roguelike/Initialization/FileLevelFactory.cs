using System;
using System.IO;
using System.Linq;
using Roguelike.Model;
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
    ///     Next lines contain information about characters:
    ///         1. Character symbol:
    ///             $ -- player start position
    ///             * --- aggressive mob
    ///             @ --- passive mob
    ///             % --- coward mob
    ///             ? --- confused player
    ///             o --- confused mob
    ///         2. Position (Y, X)
    ///         3. Statistics (Experience, Force, Health)
    /// </summary>
    public class FileLevelFactory : ILevelFactory
    {
        public const string Wall = "#";
        public const string Empty = ".";
        private const string Player = "$";
        private const string AggressiveMob = "*";
        private const string PassiveMob = "@";
        private const string CowardMob = "%";
        private const string ConfusedPlayer = "?";
        private const string ConfusedMob = "o";

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
                    var inputRow = s.Split();
                    var input = inputRow[0];
                    var positions = inputRow.Skip(1).Take(2).Select(int.Parse).ToArray();
                    var statistics = inputRow.Skip(3).Take(3).Select(int.Parse).ToArray();
                    var position = new Position(positions[0], positions[1]);
                    var statistic = new CharacterStatistics(statistics[0], statistics[1], statistics[2]);
                    boardTable[position.Y, position.X] = GetCharacter(input, level, position, statistic);
                }

                return new Board(width, height, boardTable);
            });
        }

        private static GameObject GetBoardObject(string input, Position position)
        {
            switch (input)
            {
                case Wall:
                    return new Wall(position);
                default:
                    return new EmptyCell(position);
            }
        }

        private static Character GetCharacter(string input, Level level, Position position, CharacterStatistics statistics)
        {
            switch (input)
            {
                case Player:
                    return new Player(level, position, statistics);
                case ConfusedPlayer:
                    return new ConfusedPlayer(level, new Player(level, position, statistics));
                case AggressiveMob:
                    return new Mob(level, new AggressiveMobBehaviour(), position, statistics);
                case CowardMob:
                    return new Mob(level, new CowardMobBehaviour(), position, statistics);
                case PassiveMob:
                    return new Mob(level, new PassiveMobBehaviour(), position, statistics);
                case ConfusedMob:
                    return new Mob(level, new PassiveMobBehaviour(), position, statistics, true);
                default:
                    throw new ArgumentException($"Unknown character: {input}.");
            }
        }
    }
}