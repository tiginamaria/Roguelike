using NUnit.Framework;
using Roguelike;
using Roguelike.Initialization;
using Roguelike.Model;

namespace RoguelikeTest
{
    [TestFixture]
    public class LevelTests
    {

        [Test]
        public void LevelGenerationTest()
        {
            const int height = 3;
            const int width = 6;
            var boardConfiguration = new[] {
                new[]{'#', '.', '#', '#', '#', '.'}, 
                new[]{'#', '#', '.', '#', '#', '.'}, 
                new[]{'#', '.', '#', '.', '$', '#'}
            };
            var path = TestUtils.WriteToFile(boardConfiguration, "level_test.txt");
            var level = new FileLevelFactory(path).CreateLevel();
            Assert.AreEqual(height, level.Board.Height);
            Assert.AreEqual(width, level.Board.Width);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    switch (boardConfiguration[i][j])
                    {
                        case '#':
                            Assert.IsTrue(level.Board.IsWall(new Position(i, j)));
                            break;
                        case '.':
                            Assert.IsTrue(level.Board.IsEmpty(new Position(i, j)));
                            break;
                        case '$':
                            Assert.AreEqual(level.Player.Position, new Position(i, j));
                            break;
                    }
                }
            }
        }

        [Test]
        public void BoardConfigurationTest()
        {
            const int height = 3;
            const int width = 6;
            var boardConfiguration = new[]
            {
                new[] {'#', '.', '#', '#', '#', '.'},
                new[] {'#', '#', '.', '#', '#', '.'},
                new[] {'#', '.', '#', '.', '$', '#'}
            };
            var path = TestUtils.WriteToFile(boardConfiguration, "level_test.txt");
            var board = new FileLevelFactory(path).CreateLevel().Board;
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Assert.IsTrue(board.CheckOnBoard(new Position(i, j)));
                }
            }
            for (var i = 0; i < height; i++)
            {
                Assert.IsFalse(board.CheckOnBoard(new Position(i, -1)));
                Assert.IsFalse(board.CheckOnBoard(new Position(i, width)));
            }
            for (var j = 0; j < width; j++)
            {
                Assert.IsFalse(board.CheckOnBoard(new Position(-1, j)));
                Assert.IsFalse(board.CheckOnBoard(new Position(height, j)));
            }
        }
    }
}
