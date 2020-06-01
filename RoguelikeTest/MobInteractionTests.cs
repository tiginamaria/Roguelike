using System;
using System.IO;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.Mobs;
using Roguelike.Model.PlayerModel;
using Roguelike.View;

namespace RoguelikeTest
{
    [TestFixture]
    public class MobInteractionTests
    {
        private Level level;
        
        [SetUp]
        public void SetUp()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/mob_interaction_test_map.txt");
            level = new FileLevelFactory(path).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");
        }

        [Test]
        public void NetworkMobMoveInteractorTest()
        {
            var playView = new VoidView();
            var mobMoveInteractor = new NetworkMobMoveInteractor(level, playView);
            var cowardMob = level.Mobs[0];
            
            var oldPosition = cowardMob.Position;
            mobMoveInteractor.IntentMove(cowardMob, 1, 0);
            var newPosition = cowardMob.Position;

            Assert.AreEqual(oldPosition.X, newPosition.X);
            Assert.AreEqual(oldPosition.Y, newPosition.Y);
            
            var aggressiveMob = level.Mobs[5];
            
            oldPosition = aggressiveMob.Position;
            mobMoveInteractor.IntentMove(aggressiveMob, 0, 1);
            newPosition = aggressiveMob.Position;

            Assert.AreEqual(oldPosition.X, newPosition.X);
            Assert.AreEqual(oldPosition.Y, newPosition.Y);
        }

        [Test]
        public void MobMoveInteractorTest()
        {
            var playView = new VoidView();
            var mobMoveInteractor = new MobMoveInteractor(level, playView);
            
            var cowardMob = level.Mobs[0];
           
            var oldPosition = cowardMob.Position;
            mobMoveInteractor.IntentMove(cowardMob, 1, 0);
            var newPosition = cowardMob.Position;

            Assert.AreEqual(oldPosition.X, newPosition.X);
            Assert.AreEqual(oldPosition.Y, newPosition.Y);
            
            var aggressiveMob = level.Mobs[5];
            
            oldPosition = aggressiveMob.Position;
            mobMoveInteractor.IntentMove(aggressiveMob, 0, 1);
            newPosition = aggressiveMob.Position;

            Assert.AreEqual(oldPosition.X, newPosition.X);
            Assert.AreEqual(oldPosition.Y, newPosition.Y);
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
            
            var networkMob = level.Mobs[4];
            var newPosition = networkMob.GetBehaviour().MakeMove(level, networkMob.Position);
            Assert.AreEqual(new Position(2, 5),  newPosition);
        }

        [Test]
        public void CowardMobMoveTest()
        {
            var cowardMob = level.Mobs[0];
            Assert.IsInstanceOf(typeof(CowardMobBehaviour), cowardMob.GetBehaviour());
            var newPosition = cowardMob.GetBehaviour().MakeMove(level, cowardMob.Position);
            Assert.AreEqual(new Position(0, 5),  newPosition);
        }
        
        [Test]
        public void PassiveMobMoveTest()
        {
            var passiveMob = level.Mobs[1];
            Assert.IsInstanceOf(typeof(PassiveMobBehaviour), passiveMob.GetBehaviour());
            var oldPosition = passiveMob.Position;
            var newPosition = passiveMob.GetBehaviour().MakeMove(level, passiveMob.Position);
            Assert.AreEqual(oldPosition,  newPosition);
        }
                
        [Test]
        public void ConfusedMobMoveTest()
        {
            var confusedMob = level.Mobs[2];
            Assert.IsInstanceOf(typeof(ConfusedMobBehaviour), confusedMob.GetBehaviour());
            var newPosition = confusedMob.GetBehaviour().MakeMove(level, confusedMob.Position);
            Assert.IsTrue(Math.Abs(confusedMob.Position.X - newPosition.X) <= 1);
            Assert.IsTrue(Math.Abs(confusedMob.Position.Y - newPosition.Y) <= 1);
        }

        [Test]
        public void AggressiveMobMoveTest()
        {
            var aggressiveMob = level.Mobs[4];
            Assert.IsInstanceOf(typeof(AggressiveMobBehaviour), aggressiveMob.GetBehaviour());
            var newPosition = aggressiveMob.GetBehaviour().MakeMove(level, aggressiveMob.Position);
            Assert.AreEqual(new Position(2, 5),  newPosition);
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