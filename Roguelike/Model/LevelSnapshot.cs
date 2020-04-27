using System.Collections.Generic;
using System.IO;
using System.Text;
using Roguelike.Model.Objects;
using Roguelike.Model.PlayerModel;
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
                            var statistics = character.GetStatistics();
                            configurations.Add($"{statistics.Experience} {statistics.Force} {statistics.Health}");
                        }
                        var c = board.IsEmpty(position) ? '.' : ConsolePlayView.GetObjectChar(board, position);
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