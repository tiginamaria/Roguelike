using System;
using System.IO;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Model;

namespace RoguelikeTest
{

    [TestFixture]
    public class PlayerInteractionTests
    {
        private Level level;
        [SetUp]
        public void SetUp()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/player_interaction_test_map.txt");
            level = new FileLevelFactory(path).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");
        }

        [Test]
        public void PlayerMoveToEmptyPositionTest()
        {
            var board = level.Board;
            var player = level.CurrentPlayer;
            var oldPlayerPosition = player.Position;
            var newPlayerPosition = new Position(player.Position.Y + 1, player.Position.X);
            level.CurrentPlayer.Move(1, 0, board);
            Assert.IsTrue(board.IsEmpty(oldPlayerPosition));
            Assert.AreEqual(newPlayerPosition, level.CurrentPlayer.Position);
        }
        
        [Test]
        public void PlayerMoveToWallPositionTest()
        {
            var board = level.Board;
            var player = level.CurrentPlayer;
            var oldPlayerPosition = player.Position;
            level.CurrentPlayer.Move(0, -1, board);
            Assert.AreEqual(oldPlayerPosition, level.CurrentPlayer.Position);
        }
        
        [Test]
        public void PlayerMoveOutOfBoardTest()
        {
            var board = level.Board;
            var oldPlayerPosition = level.CurrentPlayer.Position;
            var nextPlayerPosition = new Position(level.CurrentPlayer.Position.Y, level.CurrentPlayer.Position.X + 1);
            level.CurrentPlayer.Move(0, 1, board);
            Assert.IsTrue(board.IsEmpty(oldPlayerPosition));
            Assert.AreEqual(nextPlayerPosition, level.CurrentPlayer.Position);
            
            level.CurrentPlayer.Move(0, 1, board);
            Assert.AreEqual(nextPlayerPosition, level.CurrentPlayer.Position);
        }
    }
}
