using System.IO;
using NUnit.Framework;
using Roguelike;
using Roguelike.Initialization;
using Roguelike.Model;

namespace RoguelikeTest
{

    [TestFixture]
    public class InteractionTests
    {
        private string path;
        
        [SetUp]
        public void SetUp()
        {
            var boardConfiguration = new[]
            {
                new[] {'#', '.', '#', '#', '#', '.'},
                new[] {'#', '#', '.', '#', '#', '.'},
                new[] {'#', '.', '#', '.', '$', '#'}
            };
            path = TestUtils.WriteToFile(boardConfiguration, "interaction_test.txt");
        }

        [Test]
        public void PlayerMoveToEmptyPositionTest()
        {
            var level = new FileLevelFactory(path).CreateLevel();
            var board = level.Board;
            var player= level.Player;
            var oldPlayerPosition = player.Position;
            var newPlayerPosition = new Position(player.Position.Y, player.Position.X - 1);
            level.Player.Move(0, -1, board);
            Assert.IsTrue(board.IsEmpty(oldPlayerPosition));
            Assert.AreEqual(newPlayerPosition, level.Player.Position);
        }
        
        [Test]
        public void PlayerMoveToWallPositionTest()
        {
            var level = new FileLevelFactory(path).CreateLevel();
            var board = level.Board;
            var player= level.Player;
            var oldPlayerPosition = player.Position;
            level.Player.Move(-1, 0, board);
            Assert.AreEqual(oldPlayerPosition, level.Player.Position);
        }
        
        [Test]
        public void PlayerMoveOutOfBoardTest()
        {
            var level = new FileLevelFactory(path).CreateLevel();
            var board = level.Board;
            var player= level.Player;
            var oldPlayerPosition = player.Position;
            level.Player.Move(1, 0, board);
            Assert.AreEqual(oldPlayerPosition, level.Player.Position);
        }
    }
}
