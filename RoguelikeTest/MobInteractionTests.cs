using System;
using System.IO;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.Mobs;
using Roguelike.Model.PlayerModel;

namespace RoguelikeTest
{
    [TestFixture]
    public class MobInteractionTests
    {
        
        private int height;
        private int width;
        private Level level;
        
        [SetUp]
        public void SetUp()
        {
            height = 5;
            width = 6;
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/mob_interaction_test_map.txt");
            level = new FileLevelFactory(path).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");
        }
        
        [Test]
        public void NetworkMobMoveTest()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/mob_interaction_test_map.txt");
            var factory = new FileLevelFactory(path);
            factory.SetMobFactory(new NetworkMobFactory());
            factory.SetPlayerFactory(new NetworkPlayerFactory(new ExitGameInteractor()));
            var level = factory.CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");
            
            var networkMob = level.Mobs.Find(mob => mob.GetBehaviour() is AggressiveMobBehaviour);
            var newPosition = networkMob.GetBehaviour().MakeMove(level, networkMob.Position);
            Assert.AreEqual(new Position(2, 5),  newPosition);
        }
        
        [Test]
        public void ConfusedMobMoveTest()
        {
            var confusedMob = level.Mobs.Find(mob => mob.GetBehaviour() is ConfusedMobBehaviour);
            var newPosition = confusedMob.GetBehaviour().MakeMove(level, confusedMob.Position);
            Assert.IsTrue(Math.Abs(confusedMob.Position.X - newPosition.X) <= 1);
            Assert.IsTrue(Math.Abs(confusedMob.Position.Y - newPosition.Y) <= 1);
        }

        [Test]
        public void AggressiveMobMoveTest()
        {
            var aggressiveMob = level.Mobs.Find(mob => mob.GetBehaviour() is AggressiveMobBehaviour);
            var newPosition = aggressiveMob.GetBehaviour().MakeMove(level, aggressiveMob.Position);
            Assert.AreEqual(new Position(2, 5),  newPosition);
        }
        
        [Test]
        public void CowardMobMoveTest()
        {
            var cowardMob = level.Mobs.Find(mob => mob.GetBehaviour() is CowardMobBehaviour);
            var newPosition = cowardMob.GetBehaviour().MakeMove(level, cowardMob.Position);
            Assert.AreEqual(new Position(0, 5),  newPosition);
        }
        
        [Test]
        public void PassiveMobMoveTest()
        {
            var passiveMob = level.Mobs.Find(mob => mob.GetBehaviour() is PassiveMobBehaviour);
            var oldPosition = passiveMob.Position;
            var newPosition = passiveMob.GetBehaviour().MakeMove(level, passiveMob.Position);
            Assert.AreEqual(oldPosition,  newPosition);
        }
        private void MoveMobOnEmptyField(Level level, Mob mob, int dx, int dy)
        {
            var oldMobPosition = mob.Position;
            var newMObPosition = new Position(mob.Position.Y + dy, mob.Position.X + dx);
            mob.Move(dy, dx, level.Board);
            Assert.IsTrue(level.Board.IsEmpty(oldMobPosition));
            Assert.AreEqual(newMObPosition, mob.Position);
        }
        
        private void MoveMobOnWallField(Level level, Mob mob, int dx, int dy)
        {
            var oldMobPosition = mob.Position;
            var newMObPosition = new Position(mob.Position.Y + dy, mob.Position.X + dx);
            mob.Move(dy, dx, level.Board);
            Assert.IsTrue(level.Board.IsWall(newMObPosition));
            Assert.AreEqual(oldMobPosition, mob.Position);
        }

        [Test]
        public void MobMoveToEmptyPositionTest()
        {
            var mob = level.Board.GetObject( new Position(1, 1)) as Mob;
            Assert.NotNull(mob);
            
            MoveMobOnEmptyField(level, mob, -1, 0);
            MoveMobOnEmptyField(level, mob, 1, 0);
            MoveMobOnEmptyField(level, mob, 0, -1);
            MoveMobOnEmptyField(level, mob, 0, 1);
        }
        
        [Test]
        public void MobMoveToWallPositionTest()
        {
            var mob = level.Board.GetObject( new Position(1, 4)) as Mob;
            Assert.NotNull(mob);

            MoveMobOnWallField(level, mob, -1, 0);
            MoveMobOnWallField(level, mob, 1, 0);
            MoveMobOnWallField(level, mob, 0, -1);
            MoveMobOnWallField(level, mob, 0, 1);
        }
        
        [Test]
        public void MobMoveOutOfBoardTest()
        {
            var mob = level.Board.GetObject( new Position(0, 5)) as Mob;
            Assert.NotNull(mob);
            var oldMobPosition = mob.Position;
            level.CurrentPlayer.Move(1, 0, level.Board);
            Assert.AreEqual(oldMobPosition, mob.Position);
            level.CurrentPlayer.Move(0, 1, level.Board);
            Assert.AreEqual(oldMobPosition, mob.Position);
        }
    }
}