using System;
using System.IO;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.View;

namespace RoguelikeTest.test_maps
{
    public class GameInteractorsTest
    {
        private Level level;

        [SetUp]
        public void SetUp()
        {
            var path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/player_interaction_test_map.txt");
            level = new FileLevelFactory(path1).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");
        }

        [Test]
        public void ExitInteractorTest()
        {
            var exitInteractor = new ExitGameInteractor(level);
            exitInteractor.SetLevel(level);
            Assert.IsTrue(level.ContainsPlayer("testplayer"));
            exitInteractor.Exit(level.CurrentPlayer);
            Assert.IsFalse(level.ContainsPlayer("testplayer"));
        }
        
        [Test]
        public void SaveInteractorTest()
        {
            var exitInteractor = new SaveGameInteractor(level);
            exitInteractor.Save();
            exitInteractor.Dump();
            exitInteractor.DeleteSaving();
        }
        
        [Test]
        public void SpawnPlayerInteractorTest()
        {
            var playerView = new VoidView();
            var spawnPlayerInteractor = new SpawnPlayerInteractor(level, playerView);
            spawnPlayerInteractor.Spawn(new Position(1, 1), "newplayer");
            Assert.IsTrue(level.ContainsPlayer("newplayer"));
            spawnPlayerInteractor.DeletePlayer("newplayer");
            Assert.IsFalse(level.ContainsPlayer("newplayer"));
        }
        
    }
}