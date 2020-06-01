using System;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Model;
using Roguelike.Model.Mobs;
using Roguelike.Model.PlayerModel;

namespace RoguelikeTest
{

    [TestFixture]
    public class ConfusionTests
    {
        private Level level;

        [SetUp]
        public void SetUp()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/confusion_test_map.txt");
            level = new FileLevelFactory(path).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");
        }

        [Test]
        public void ConfusedPlayerMoveTest()
        {
            var confusedPlayer = new ConfusedPlayer(level, level.CurrentPlayer);
            var startPosition = confusedPlayer.Position;
            var possiblePositions = new[]
            {
                new Position(startPosition.Y, startPosition.X),
                new Position(startPosition.Y, startPosition.X + 1),
                new Position(startPosition.Y + 1, startPosition.X)
            };
            confusedPlayer.Move(1, 0, level.Board);
            var endPosition = confusedPlayer.Position;
            Assert.IsTrue(possiblePositions.Contains(endPosition));
        }

        [Test]
        public void MobConfusePlayerTest()
        {
            var mob = level.Board.GetObject(new Position(0, 1)) as Mob;
            Assert.NotNull(mob);

            var mobStatBeforeConfuse = mob.GetStatistics().Clone() as CharacterStatistics;
            var playerStatBeforeConfuse = level.CurrentPlayer.GetStatistics().Clone() as CharacterStatistics;

            mob.MakeDamage(level.CurrentPlayer);

            Assert.AreEqual(typeof(ConfusedPlayer), level.CurrentPlayer.GetType());

            Thread.Sleep(6000);
            Assert.AreEqual(typeof(Player), level.CurrentPlayer.GetType());

            var mobStatAfterConfuse = mob.GetStatistics();
            var playerStatAfterConfuse = level.CurrentPlayer.GetStatistics();

            Assert.AreEqual(mobStatBeforeConfuse.Health, mobStatAfterConfuse.Health);
            Assert.AreEqual(mobStatBeforeConfuse.Force + playerStatBeforeConfuse.Force / 2, mobStatAfterConfuse.Force);
            Assert.AreEqual(mobStatBeforeConfuse.Experience + 1, mobStatAfterConfuse.Experience);

            Assert.AreEqual(playerStatBeforeConfuse.Health - mobStatBeforeConfuse.Force / 2,
                playerStatAfterConfuse.Health);
            Assert.AreEqual(playerStatBeforeConfuse.Force, playerStatAfterConfuse.Force);
            Assert.AreEqual(playerStatBeforeConfuse.Experience - 1, playerStatAfterConfuse.Experience);
        }

        [Test]
        public void MobConfusePlayerDyeTest()
        {
            var mob = level.Board.GetObject(new Position(0, 1)) as Mob;
            Assert.NotNull(mob);

            for (var i = 0; i < 3; i++)
            {
                mob.MakeDamage(level.CurrentPlayer);
                Assert.AreEqual(typeof(ConfusedPlayer), level.CurrentPlayer.GetType());
            }

            mob.MakeDamage(level.CurrentPlayer);
            Assert.AreEqual(0, level.CurrentPlayer.GetStatistics().Health);
            Assert.AreEqual(typeof(Player), level.CurrentPlayer.GetType());
        }

        [Test]
        public void PlayerConfuseMobDyeTest()
        {
            var mobPosition = new Position(0, 1);
            var mob = level.Board.GetObject(mobPosition) as Mob;
            Assert.NotNull(mob);
            Assert.AreEqual(typeof(AggressiveMobBehaviour), mob.GetBehaviour().GetType());
            
            level.CurrentPlayer.MakeDamage(mob);
            Assert.AreEqual(typeof(ConfusedMobBehaviour), mob.GetBehaviour().GetType());
            Assert.AreEqual(typeof(Player), level.CurrentPlayer.GetType());
            Assert.AreEqual(1, mob.GetStatistics().Health);
            Thread.Sleep(6000);
            
            level.CurrentPlayer.MakeDamage(mob);
            Assert.AreEqual(0, mob.GetStatistics().Health);
            level.Board.IsEmpty(mobPosition);
        }
    }
}
