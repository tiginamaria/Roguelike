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
        public class ConfusionTests
        {
            [Test]
            public void ConfusedPlayerMoveTest()
            {
                var boardConfiguration = new[]
                {
                    new[] {'#', '#', '.'},
                    new[] {'.', '$', '.'},
                    new[] {'.', '.', '.'}
                };
                var path = TestUtils.WriteToFile(boardConfiguration, "confusion_test_map.txt");
                var level = new FileLevelFactory(path).CreateLevel();
                var confusedPlayer = new ConfusedPlayer(level, level.Player);
                var startPosition = confusedPlayer.Position;
                var possiblePositions = new[]
                {
                    new Position(startPosition.Y, startPosition.X),
                    new Position(startPosition.Y, startPosition.X + 1),
                    new Position(startPosition.Y, startPosition.X - 1),
                    new Position(startPosition.Y + 1, startPosition.X)
                };
                confusedPlayer.Move(1, 0, level.Board);
                var endPosition = confusedPlayer.Position;
                Assert.IsTrue(possiblePositions.Contains(endPosition));
            }
            
            [Test]
            public void MobConfusePlayerTest()
            {
                var boardConfiguration = new[]
                {
                    new[] {'.', '*', '.'},
                    new[] {'.', '$', '.'},
                    new[] {'.', '.', '.'}
                };
                var path = TestUtils.WriteToFile(boardConfiguration, "confusion_test_map.txt");
                var level = new FileLevelFactory(path).CreateLevel();
                var mob = level.Board.GetObject(new Position(0, 1)) as Mob;
                Assert.NotNull(mob);
                
                var mobStatBeforeConfuse = mob.GetStatistics().Clone() as CharacterStatistics;
                var playerStatBeforeConfuse = level.Player.GetStatistics().Clone() as CharacterStatistics;
                
                mob.Confuse(level.Player);
                
                Assert.AreEqual(typeof(ConfusedPlayer), level.Player.GetType());
                
                Thread.Sleep(6000);
                Assert.AreEqual(typeof(Player), level.Player.GetType());
                
                var mobStatAfterConfuse = mob.GetStatistics();
                var playerStatAfterConfuse = level.Player.GetStatistics();
                
                Assert.AreEqual(mobStatBeforeConfuse.Health, mobStatAfterConfuse.Health);
                Assert.AreEqual(mobStatBeforeConfuse.Force + playerStatBeforeConfuse.Force / 2, mobStatAfterConfuse.Force);
                Assert.AreEqual(mobStatBeforeConfuse.Experience + 1, mobStatAfterConfuse.Experience);
                
                Assert.AreEqual(playerStatBeforeConfuse.Health - mobStatBeforeConfuse.Force / 2, playerStatAfterConfuse.Health);
                Assert.AreEqual(playerStatBeforeConfuse.Force, playerStatAfterConfuse.Force);
                Assert.AreEqual(playerStatBeforeConfuse.Experience - 1, playerStatAfterConfuse.Experience);
            }
            
            [Test]
            public void MobConfusePlayerDyeTest()
            {
                var boardConfiguration = new[]
                {
                    new[] {'.', '*', '.'},
                    new[] {'.', '$', '.'},
                    new[] {'.', '.', '.'}
                };
                var path = TestUtils.WriteToFile(boardConfiguration, "confusion_test_map.txt");
                var level = new FileLevelFactory(path).CreateLevel();
                var mob = level.Board.GetObject(new Position(0, 1)) as Mob;
                Assert.NotNull(mob);

                for (var i = 0; i < 14; i++)
                {
                    mob.Confuse(level.Player);
                    Assert.AreEqual(typeof(ConfusedPlayer), level.Player.GetType());
                }
                
                mob.Confuse(level.Player);
                Assert.AreEqual(0, level.Player.GetStatistics().Health);
                Assert.AreEqual(typeof(Player), level.Player.GetType());
            }
        }
        
        [Test]
        public void PlayerConfuseMobDyeTest()
        {
            var boardConfiguration = new[]
            {
                new[] {'.', '*', '.'},
                new[] {'.', '$', '.'},
                new[] {'.', '.', '.'}
            };
            var path = TestUtils.WriteToFile(boardConfiguration, "confusion_test_map.txt");
            var level = new FileLevelFactory(path).CreateLevel();
            var mobPosition = new Position(0, 1);
            var mob = level.Board.GetObject(mobPosition) as Mob;
            Assert.NotNull(mob);

            for (var i = 0; i < 3; i++)
            {
                level.Player.Confuse(mob);
                Assert.AreEqual(typeof(ConfusedMobBehaviour), mob.Behaviour.GetType());
                Assert.AreEqual(typeof(Player), level.Player.GetType());
            }
            Assert.AreEqual(0, mob.GetStatistics().Health);
            level.Board.IsEmpty(mobPosition);
        }
    }
}