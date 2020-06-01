using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.Inventory;
using Roguelike.View;

namespace RoguelikeTest
{
    public class InventoryInteractionTest
    {
        private Level level;
        private Level confusedLevel;
        [SetUp]
        public void SetUp()
        {
            var path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/inventory_test_map.txt");
            level = new FileLevelFactory(path1).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");
            
            var path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/confused_inventory_test_map.txt");
            confusedLevel = new FileLevelFactory(path2).CreateLevel();
            confusedLevel.CurrentPlayer = confusedLevel.GetPlayer("testplayer");
        }

        [Test]
        public void InventoryInteractorPutOnTest()
        {
            var playView = new VoidView();
            var inventoryInteractor = new InventoryInteractor(level, playView);

            Assert.AreEqual(3, level.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(1, level.CurrentPlayer.GetAppliedInventory().Count);
            Assert.IsInstanceOf(typeof(IncreaseForceItem), level.CurrentPlayer.GetInventory()[2]);

            var beforeStatistics = level.CurrentPlayer.GetStatistics().Clone() as CharacterStatistics;

            inventoryInteractor.PutOn(level.CurrentPlayer, level.CurrentPlayer.GetInventory()[2].GetStringType());

            Assert.AreEqual(2, level.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(2, level.CurrentPlayer.GetAppliedInventory().Count);
            
            var afterStatistics = level.CurrentPlayer.GetStatistics();
            
            Assert.AreEqual(beforeStatistics.Health, afterStatistics.Health);
            Assert.AreEqual(beforeStatistics.Experience, afterStatistics.Experience);
            Assert.AreEqual(beforeStatistics.Force + 3, afterStatistics.Force);
        }
        
        [Test]
        public void InventoryInteractorPutOffTest()
        {
            var playView = new VoidView();
            var inventoryInteractor = new InventoryInteractor(level, playView);

            Assert.AreEqual(3, confusedLevel.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(1, confusedLevel.CurrentPlayer.GetAppliedInventory().Count);
            Assert.IsInstanceOf(typeof(IncreaseHealthItem), confusedLevel.CurrentPlayer.GetAppliedInventory()[0]);

            var beforeStatistics = confusedLevel.CurrentPlayer.GetStatistics().Clone() as CharacterStatistics;
            var inventoryPosition = confusedLevel.CurrentPlayer.GetAppliedInventory()[0].Position;
            confusedLevel.Board.IsEmpty(inventoryPosition);
            
            inventoryInteractor.PutOff(confusedLevel.CurrentPlayer, confusedLevel.CurrentPlayer.GetAppliedInventory()[0].GetStringType());
            Assert.AreEqual(3, confusedLevel.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(0, confusedLevel.CurrentPlayer.GetAppliedInventory().Count);
            
            var afterStatistics = confusedLevel.CurrentPlayer.GetStatistics();
            confusedLevel.Board.IsInventory(inventoryPosition);
            
            Assert.AreEqual(beforeStatistics.Experience, afterStatistics.Experience);
            Assert.AreEqual(beforeStatistics.Force, afterStatistics.Force);
            Assert.AreEqual(beforeStatistics.Health - 3, afterStatistics.Health);
        }

        [Test]
        public void PlayerSavedInventoryTest()
        {
            var inventory = level.CurrentPlayer.GetInventory();
            Assert.AreEqual(3, inventory.Count);
            Assert.AreEqual(2, inventory.Count(inventory => inventory is IncreaseAllItem));
            Assert.AreEqual(1, inventory.Count(inventory => inventory is IncreaseForceItem));

            var appliedInventory = level.CurrentPlayer.GetAppliedInventory();
            Assert.AreEqual(1, appliedInventory.Count);
            Assert.AreEqual(1, appliedInventory.Count(inventory => inventory is IncreaseHealthItem));
        }
        
        [Test]
        public void PlayerApplyInventoryTest()
        {
            Assert.AreEqual(3, level.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(1, level.CurrentPlayer.GetAppliedInventory().Count);
            Assert.IsInstanceOf(typeof(IncreaseForceItem), level.CurrentPlayer.GetInventory()[2]);

            var beforeStatistics = level.CurrentPlayer.GetStatistics().Clone() as CharacterStatistics;
            
            level.CurrentPlayer.PutOn(level.CurrentPlayer.GetInventory()[2].GetStringType());
            Assert.AreEqual(2, level.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(2, level.CurrentPlayer.GetAppliedInventory().Count);
            
            var afterStatistics = level.CurrentPlayer.GetStatistics();
            
            Assert.AreEqual(beforeStatistics.Health, afterStatistics.Health);
            Assert.AreEqual(beforeStatistics.Experience, afterStatistics.Experience);
            Assert.AreEqual(beforeStatistics.Force + 3, afterStatistics.Force);
        }
        
        [Test]
        public void PlayerRemoveInventoryTest()
        {
            Assert.AreEqual(3, confusedLevel.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(1, confusedLevel.CurrentPlayer.GetAppliedInventory().Count);
            Assert.IsInstanceOf(typeof(IncreaseHealthItem), confusedLevel.CurrentPlayer.GetAppliedInventory()[0]);

            var beforeStatistics = confusedLevel.CurrentPlayer.GetStatistics().Clone() as CharacterStatistics;
            var inventoryPosition = confusedLevel.CurrentPlayer.GetAppliedInventory()[0].Position;
            confusedLevel.Board.IsEmpty(inventoryPosition);
            
            confusedLevel.CurrentPlayer.PutOff(confusedLevel.CurrentPlayer.GetAppliedInventory()[0].GetStringType());
            Assert.AreEqual(3, confusedLevel.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(0, confusedLevel.CurrentPlayer.GetAppliedInventory().Count);
            
            var afterStatistics = confusedLevel.CurrentPlayer.GetStatistics();
            confusedLevel.Board.IsInventory(inventoryPosition);
            
            Assert.AreEqual(beforeStatistics.Experience, afterStatistics.Experience);
            Assert.AreEqual(beforeStatistics.Force, afterStatistics.Force);
            Assert.AreEqual(beforeStatistics.Health - 3, afterStatistics.Health);
        }
        
        [Test]
        public void ConfusedPlayerApplyInventoryTest()
        {
            Assert.AreEqual(3, confusedLevel.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(1, confusedLevel.CurrentPlayer.GetAppliedInventory().Count);
            Assert.IsInstanceOf(typeof(IncreaseForceItem), confusedLevel.CurrentPlayer.GetInventory()[2]);

            var beforeStatistics = confusedLevel.CurrentPlayer.GetStatistics().Clone() as CharacterStatistics;
            
            confusedLevel.CurrentPlayer.PutOn(confusedLevel.CurrentPlayer.GetInventory()[2].GetStringType());
            Assert.AreEqual(2, confusedLevel.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(2, confusedLevel.CurrentPlayer.GetAppliedInventory().Count);
            
            var afterStatistics = confusedLevel.CurrentPlayer.GetStatistics();
            
            Assert.AreEqual(beforeStatistics.Health, afterStatistics.Health);
            Assert.AreEqual(beforeStatistics.Experience, afterStatistics.Experience);
            Assert.AreEqual(beforeStatistics.Force + 3, afterStatistics.Force);
        }
        
        [Test]
        public void ConfusedPlayerRemoveInventoryTest()
        {
            Assert.AreEqual(3, confusedLevel.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(1, confusedLevel.CurrentPlayer.GetAppliedInventory().Count);
            Assert.IsInstanceOf(typeof(IncreaseHealthItem), confusedLevel.CurrentPlayer.GetAppliedInventory()[0]);

            var beforeStatistics = confusedLevel.CurrentPlayer.GetStatistics().Clone() as CharacterStatistics;
            var inventoryPosition = confusedLevel.CurrentPlayer.GetAppliedInventory()[0].Position;
            confusedLevel.Board.IsEmpty(inventoryPosition);
            
            confusedLevel.CurrentPlayer.PutOff(confusedLevel.CurrentPlayer.GetAppliedInventory()[0].GetStringType());
            Assert.AreEqual(3, confusedLevel.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(0, confusedLevel.CurrentPlayer.GetAppliedInventory().Count);
            
            var afterStatistics = confusedLevel.CurrentPlayer.GetStatistics();
            confusedLevel.Board.IsInventory(inventoryPosition);
            
            Assert.AreEqual(beforeStatistics.Experience, afterStatistics.Experience);
            Assert.AreEqual(beforeStatistics.Force, afterStatistics.Force);
            Assert.AreEqual(beforeStatistics.Health - 3, afterStatistics.Health);
        }
    }
}