using System;
using System.IO;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Model;
using Roguelike.Model.Mobs;

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

        private void moveMobOnEmptyField(Level level, Mob mob, int dx, int dy)
        {
            var oldMobPosition = mob.Position;
            var newMObPosition = new Position(mob.Position.Y + dy, mob.Position.X + dx);
            mob.Move(dy, dx, level.Board);
            Assert.IsTrue(level.Board.IsEmpty(oldMobPosition));
            Assert.AreEqual(newMObPosition, mob.Position);
        }
        
        private void moveMobOnWallField(Level level, Mob mob, int dx, int dy)
        {
            var oldMobPosition = mob.Position;
            var newMObPosition = new Position(mob.Position.Y + dy, mob.Position.X + dx);
            mob.Move(dy, dx, level.Board);
            Assert.IsTrue(level.Board.IsWall(newMObPosition));
            Assert.AreEqual(oldMobPosition, mob.Position);
        }

        [Test]
        public void MObMoveToEmptyPositionTest()
        {
            var mob = level.Board.GetObject( new Position(1, 1)) as Mob;
            Assert.NotNull(mob);
            
            moveMobOnEmptyField(level, mob, -1, 0);
            moveMobOnEmptyField(level, mob, 1, 0);
            moveMobOnEmptyField(level, mob, 0, -1);
            moveMobOnEmptyField(level, mob, 0, 1);
        }
        
        [Test]
        public void MobMoveToWallPositionTest()
        {
            var mob = level.Board.GetObject( new Position(1, 4)) as Mob;
            Assert.NotNull(mob);

            moveMobOnWallField(level, mob, -1, 0);
            moveMobOnWallField(level, mob, 1, 0);
            moveMobOnWallField(level, mob, 0, -1);
            moveMobOnWallField(level, mob, 0, 1);
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