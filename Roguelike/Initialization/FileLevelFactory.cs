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
    /// </summary>
    public class FileLevelFactory : ILevelFactory
    {
        private const string Wall = "#";
        private const string Empty = ".";
        private const string Player = "$";
        private const string AggressiveMob = "*";
        private const string PassiveMob = "@";
        private const string CowardMob = "%";

        private string pathToFile;

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
            return new Level(level =>
            {
                for (var row = 0; row < height; row++)
                {
                    var inputRow = lines[row].Split().ToArray();
                    for (var col = 0; col < width; col++)
                    {
                        boardTable[row, col] = GetObject(level, inputRow[col], new Position(row, col));
                    }
                }

                return new Board(width, height, boardTable);
            });
        }

        private GameObject GetObject(Level level, string input, Position position)
        {
            switch (input)
            {
                case Wall:
                    return new Wall(position);
                case Player:
                    return new Player(level, position);
                case AggressiveMob:
                    return new Mob(level, new AggressiveMobBehaviour(), position);
                case CowardMob:
                    return new Mob(level, new CowardMobBehaviour(), position);
                case PassiveMob:
                    return new Mob(level, new PassiveMobBehaviour(), position);
                default:
                    return new EmptyCell(position);
            }
        }
    }
}
