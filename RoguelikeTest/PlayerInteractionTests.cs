using System;
using System.IO;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.View;

namespace RoguelikeTest
{

    [TestFixture]
    public class PlayerInteractionTests
    {
        private Level level;
        private Level confusedLevel;
        [SetUp]
        public void SetUp()
        {
            var path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/player_interaction_test_map.txt");
            level = new FileLevelFactory(path1).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");
            
            var path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/confused_player_interaction_test_map.txt");
            confusedLevel = new FileLevelFactory(path2).CreateLevel();
            confusedLevel.CurrentPlayer = confusedLevel.GetPlayer("testplayer");
        }
        
        [Test]
        public void NetworkPlayerMoveInteractorTest()
        {
            var playView = new VoidView();
            var playerMoveInteractor = new NetworkPlayerMoveInteractor(level, playView);

            var oldPosition = level.CurrentPlayer.Position;
            playerMoveInteractor.IntentMove(level.CurrentPlayer, 0, 1);
            var newPosition = level.CurrentPlayer.Position;
            
            Assert.AreEqual(oldPosition.X + 1, newPosition.X);
            Assert.AreEqual(oldPosition.Y, newPosition.Y);

            oldPosition = level.Mobs[0].Position;
            playerMoveInteractor.IntentMove(level.Mobs[0], 1, 0);
            newPosition = level.Mobs[0].Position;
            
            Assert.AreEqual(oldPosition.X, newPosition.X);
            Assert.AreEqual(oldPosition.Y + 1, newPosition.Y);
        }
        
        [Test]
        public void PlayerMoveInteractorTest()
        {
            var playView = new VoidView();
            var playerMoveInteractor = new PlayerMoveInteractor(level, playView);

            var oldPosition = level.CurrentPlayer.Position;
            playerMoveInteractor.IntentMove(level.CurrentPlayer, 0, 1);
            var newPosition = level.CurrentPlayer.Position;
            
            Assert.AreEqual(oldPosition.X + 1, newPosition.X);
            Assert.AreEqual(oldPosition.Y, newPosition.Y);
        }
        
        [Test]
        public void ConfusedPlayerMoveInteractorTest()
        {
            var playView = new VoidView();
            var playerMoveInteractor = new PlayerMoveInteractor(confusedLevel, playView);

            var oldPosition = confusedLevel.CurrentPlayer.Position;
            playerMoveInteractor.IntentMove(confusedLevel.CurrentPlayer, 0, 1);
            var newPosition = confusedLevel.CurrentPlayer.Position;
            
            Assert.IsTrue(Math.Abs(oldPosition.X - newPosition.X) <= 1);
            Assert.IsTrue(Math.Abs(oldPosition.Y - newPosition.Y) <= 1);
        }
        
        [Test]
        public void ConfusedNetworkPlayerMoveInteractorTest()
        {
            var playView = new VoidView();
            var playerMoveInteractor = new NetworkPlayerMoveInteractor(confusedLevel, playView);

            var oldPosition = confusedLevel.CurrentPlayer.Position;
            playerMoveInteractor.IntentMove(confusedLevel.CurrentPlayer, 0, 1);
            var newPosition = confusedLevel.CurrentPlayer.Position;
            
            Assert.IsTrue(Math.Abs(oldPosition.X - newPosition.X) <= 1);
            Assert.IsTrue(Math.Abs(oldPosition.Y - newPosition.Y) <= 1);
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
            
            level.CurrentPlayer.Move(0, 0, board);
            Assert.AreEqual(oldPlayerPosition, level.CurrentPlayer.Position);
            
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
