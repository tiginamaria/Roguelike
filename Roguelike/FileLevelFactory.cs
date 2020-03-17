using System.IO;
using System.Linq;

namespace Roguelike
{
    /// <inheritdoc />
    /// <summary>
    /// Creates a level by the given file.
    /// The first line must contain two integers: height and width.
    /// The next height lines contains width characters separated with spaces:
    /// # -- wall
    /// . -- empty
    /// $ -- player start position
    /// </summary>
    public class FileLevelFactory : ILevelFactory
    {
        private const string Wall = "#";
        private const string Empty = ".";
        private const string Player = "$";
        
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
            for (var row = 0; row < height; row++)
            {
                var inputRow = lines[row].Split().ToArray();
                for (var col = 0; col < width; col++)
                {
                    boardTable[row, col] = GetObject(inputRow[col], new Position(row, col));
                }
            }

            var board = new Board(width, height, boardTable);
            return new Level(board);
        }

        private GameObject GetObject(string input, Position position)
        {
            switch (input)
            {
                case Wall:
                    return new Wall(position);
                case Player:
                    return new Player(position);
                default:
                    return new EmptyCell(position);
            }
        }
    }
}