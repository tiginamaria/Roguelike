using System.Collections.Generic;
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
    /// Creates a level by the given file.
    /// The first line must contain two integers: height and width.
    /// The next height lines contains width characters separated with spaces:
    /// # -- wall
    /// . -- empty
    /// $ -- player start position
    /// * --- aggressive mob
    /// @ --- passive mob
    /// % --- coward mob
    /// ? --- confused player
    /// o --- confused mob
    /// Next lines contain triple (Experience, Force, Health) for every character on the board
    /// (in order top to bottom left to right)
    /// </summary>
    public class FileLevelFactory : ILevelFactory
    {
        private const string Wall = "#";
        private const string Empty = ".";
        private const string Player = "$";
        private const string AggressiveMob = "*";
        private const string PassiveMob = "@";
        private const string CowardMob = "%";
        private const string ConfusedPlayer = "?";
        private const string ConfusedMob = "o";

        private readonly string pathToFile;

        /// <summary>
        /// Creates the factory by the path to a file with a level description.
        /// </summary>
        public FileLevelFactory(string path)
        {
            pathToFile = path;
        }

        public Level CreateLevel()
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
            var statistics = lines
                .Skip(height)
                .Select((s) =>
                {
                    var stats = s
                        .Split()
                        .Select(int.Parse)
                        .ToArray();
                    return new CharacterStatistics(stats[0], stats[1], stats[2]);
                }).GetEnumerator();
            return new Level(level =>
            {
                for (var row = 0; row < height; row++)
                {
                    var inputRow = lines[row].Split().ToArray();
                    for (var col = 0; col < width; col++)
                    {
                        var gameObject = GetObject(level, inputRow[col], new Position(row, col), statistics);
                        boardTable[row, col] = gameObject;
                    }
                }

                return new Board(width, height, boardTable);
            });
        }

        private static GameObject GetObject(Level level, string input, Position position,
            IEnumerator<CharacterStatistics> statistics)
        {
            switch (input)
            {
                case Wall:
                    return new Wall(position);
                case Player:
                    statistics.MoveNext();
                    return new Player(level, position, statistics.Current);
                case ConfusedPlayer:
                    statistics.MoveNext();
                    return new ConfusedPlayer(level, new Player(level, position, statistics.Current));
                case AggressiveMob:
                    statistics.MoveNext();
                    return new Mob(level, new AggressiveMobBehaviour(), position, statistics.Current);
                case CowardMob:
                    statistics.MoveNext();
                    return new Mob(level, new CowardMobBehaviour(), position, statistics.Current);
                case PassiveMob:
                    statistics.MoveNext();
                    return new Mob(level, new PassiveMobBehaviour(), position, statistics.Current);
                case ConfusedMob:
                    statistics.MoveNext();
                    return new Mob(level, new PassiveMobBehaviour(), position, statistics.Current, true);
                default:
                    return new EmptyCell(position);
            }
        }
    }
}
