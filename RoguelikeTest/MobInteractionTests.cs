using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Model;
using Roguelike.Model.Mobs;

namespace TestRoguelike
{

    [TestFixture]
    public class MobInteractionTests
    {
        private string path;
        
        [SetUp]
        public void SetUp()
        {
            var boardConfiguration = new[]
            {
                new[] {'#', '.', '#', '#', '#', '%'},
                new[] {'.', '@', '.', '#', '%', '#'},
                new[] {'.', '.', '#', '$', '#', '#'}
            };
            path = TestUtils.WriteToFile(boardConfiguration, "mob_interaction_test_map.txt");
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
            var level = new FileLevelFactory(path).CreateLevel();
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
            var level = new FileLevelFactory(path).CreateLevel();
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
            var level = new FileLevelFactory(path).CreateLevel();
            var mob = level.Board.GetObject( new Position(0, 5)) as Mob;
            Assert.NotNull(mob);
            var oldMobPosition = mob.Position;
            level.Player.Move(1, 0, level.Board);
            Assert.AreEqual(oldMobPosition, mob.Position);
            level.Player.Move(0, 1, level.Board);
            Assert.AreEqual(oldMobPosition, mob.Position);
        }
    }
}