﻿using System.Linq;
using System.Threading;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Model;
using Roguelike.Model.Mobs;
using Roguelike.Model.PlayerModel;

namespace TestRoguelike
{
    public class PlayerTest
    {
        [TestFixture]
        public class InteractionTests
        {
            [Test]
            public void ConfusedPlayerMoveTest()
            {
                var boardConfiguration = new[]
                {
                    new[] {'.', '#', '.'},
                    new[] {'.', '$', '.'},
                    new[] {'.', '.', '.'}
                };
                var path = TestUtils.WriteToFile(boardConfiguration, "player_test_map.txt");
                var level = new FileLevelFactory(path).CreateLevel();
                var confusedPlayer = new ConfusedPlayer(level, level.Player);
                var startPosition = confusedPlayer.Position;
                var possiblePositions = new[]
                {
                    new Position(startPosition.Y, startPosition.X),
                    new Position(startPosition.Y, startPosition.X + 1),
                    new Position(startPosition.Y, startPosition.X - 1),
                    new Position(startPosition.Y - 1, startPosition.X)
                };
                confusedPlayer.Move(1, 0, level.Board);
                var endPosition = confusedPlayer.Position;
                Assert.IsTrue(possiblePositions.Contains(endPosition));
            }
            
            [Test]
            public void ConfusedEffectPlayerTest()
            {
                var boardConfiguration = new[]
                {
                    new[] {'.', '*', '.'},
                    new[] {'.', '$', '.'},
                    new[] {'.', '.', '.'}
                };
                var path = TestUtils.WriteToFile(boardConfiguration, "player_test_map.txt");
                var level = new FileLevelFactory(path).CreateLevel();
                level.Player.AcceptConfuse(new Mob(level, new AggressiveMobBehaviour(), new Position(2, 1)));
                Assert.AreEqual(typeof(ConfusedPlayer), level.Player.GetType());
                Thread.Sleep(6000);
                Assert.AreEqual(typeof(Player), level.Player.GetType());
            }
        }
    }
}