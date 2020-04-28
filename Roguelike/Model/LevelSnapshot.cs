using System.Collections.Generic;
using System.IO;
using Roguelike.Initialization;
using Roguelike.Model.Objects;
using Roguelike.View;

namespace Roguelike.Model
{
    public class LevelSnapshot
    {
        private readonly Board board;

        public LevelSnapshot(Board board)
        {
            this.board = board;
        }

        public void Dump(string path)
        {
            using (var txtWriter = new StreamWriter(File.Open(path, FileMode.Create)))
            {
                var configurations = new List<string>();
                txtWriter.Write($"{board.Height} {board.Width}");
                txtWriter.WriteLine();
                for (var row = 0; row < board.Height; row++)
                {
                    for (var col = 0; col < board.Width; col++)
                    {
                        var position = new Position(row, col);
                        var gameObject = board.GetObject(position);

                        if (gameObject is Character character)
                        {
                            var objectChar = ConsolePlayView.GetObjectChar(board, position);
                            var statistics = character.GetStatistics();
                            var positionString = $"{position.Y} {position.X}";
                            var statisticsString = $"{statistics.Experience} {statistics.Force} {statistics.Health}";
                            configurations.Add($"{objectChar} {positionString} {statisticsString}");
                        }

                        var c = board.IsWall(position) ? BoardObject.Wall : BoardObject.Empty;
                        txtWriter.Write($"{c} ");
                    }

                    txtWriter.WriteLine();
                }

                configurations.ForEach(s =>
                {
                    txtWriter.Write(s);
                    txtWriter.WriteLine();
                });
            }
        }
    }
}