﻿﻿﻿using System;
   using System.IO;
   using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Model;

namespace TestRoguelike
{

    [TestFixture]
    public class PlayerInteractionTests
    {
        private Level level;
        [SetUp]
        public void SetUp()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../test_maps/player_interaction_test_map.txt");
            level = new FileLevelFactory(path).CreateLevel();
        }

        [Test]
        public void PlayerMoveToEmptyPositionTest()
        {
            var board = level.Board;
            var player = level.Player;
            var oldPlayerPosition = player.Position;
            var newPlayerPosition = new Position(player.Position.Y + 1, player.Position.X);
            level.Player.Move(1, 0, board);
            Assert.IsTrue(board.IsEmpty(oldPlayerPosition));
            Assert.AreEqual(newPlayerPosition, level.Player.Position);
            Assert.IsTrue(false);
        }
        
        [Test]
        public void PlayerMoveToWallPositionTest()
        {
            var board = level.Board;
            var player = level.Player;
            var oldPlayerPosition = player.Position;
            level.Player.Move(0, -1, board);
            Assert.AreEqual(oldPlayerPosition, level.Player.Position);
        }
        
        [Test]
        public void PlayerMoveOutOfBoardTest()
        {
            var board = level.Board;
            var player = level.Player;
            var oldPlayerPosition = player.Position;
            var nextPlayerPosition = new Position(player.Position.Y, player.Position.X + 1);
            level.Player.Move(0, 1, board);
            Assert.IsTrue(board.IsEmpty(oldPlayerPosition));
            Assert.AreEqual(nextPlayerPosition, level.Player.Position);
            
            level.Player.Move(0, 1, board);
            Assert.AreEqual(nextPlayerPosition, level.Player.Position);
        }
    }
}
