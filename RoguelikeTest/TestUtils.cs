using System;
using System.IO;
using System.Linq;

namespace RoguelikeTest
{
    public class TestUtils
    {
        public static string WriteToFile(char[][] board, string filename)
        {
            var height = board.Length;
            var width = board.Length == 0 ? 0 : board[0].Length;

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"../../test_maps/{filename}");
            var fi = new FileInfo(path);
            using (TextWriter txtWriter = new StreamWriter(fi.Open(FileMode.Truncate)))
            {
                txtWriter.Write($"{height} {width}\n");
                txtWriter.Write(string.Join("\n",
                    board.Select(row => string.Join(" ", row))));
            }

            return path;
        }
    }
}