using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Model;
using Roguelike.Model.Inventory;

namespace RoguelikeTest
{
    public class InventoryTest
    {
        [Test]
        public void PlayerSavedIntentoryTest()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/inventory_test_map.txt");
            var level = new FileLevelFactory(path).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");

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
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/inventory_test_map.txt");
            var level = new FileLevelFactory(path).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");
            
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
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/confused_inventory_test_map.txt");
            var level = new FileLevelFactory(path).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");

            
            Assert.AreEqual(3, level.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(1, level.CurrentPlayer.GetAppliedInventory().Count);
            Assert.IsInstanceOf(typeof(IncreaseHealthItem), level.CurrentPlayer.GetAppliedInventory()[0]);

            var beforeStatistics = level.CurrentPlayer.GetStatistics().Clone() as CharacterStatistics;
            var inventoryPosition = level.CurrentPlayer.GetAppliedInventory()[0].Position;
            level.Board.IsEmpty(inventoryPosition);
            
            level.CurrentPlayer.PutOff(level.CurrentPlayer.GetAppliedInventory()[0].GetStringType());
            Assert.AreEqual(3, level.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(0, level.CurrentPlayer.GetAppliedInventory().Count);
            
            var afterStatistics = level.CurrentPlayer.GetStatistics();
            level.Board.IsInventory(inventoryPosition);
            
            Assert.AreEqual(beforeStatistics.Experience, afterStatistics.Experience);
            Assert.AreEqual(beforeStatistics.Force, afterStatistics.Force);
            Assert.AreEqual(beforeStatistics.Health - 3, afterStatistics.Health);
        }
        
                [Test]
        public void confusedPlayerApplyInventoryTest()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/confused_inventory_test_map.txt");
            var level = new FileLevelFactory(path).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");

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
        public void ConfusedPlayerRemoveInventoryTest()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../test_maps/confused_inventory_test_map.txt");
            var level = new FileLevelFactory(path).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");
            
            Assert.AreEqual(3, level.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(1, level.CurrentPlayer.GetAppliedInventory().Count);
            Assert.IsInstanceOf(typeof(IncreaseHealthItem), level.CurrentPlayer.GetAppliedInventory()[0]);

            var beforeStatistics = level.CurrentPlayer.GetStatistics().Clone() as CharacterStatistics;
            var inventoryPosition = level.CurrentPlayer.GetAppliedInventory()[0].Position;
            level.Board.IsEmpty(inventoryPosition);
            
            level.CurrentPlayer.PutOff(level.CurrentPlayer.GetAppliedInventory()[0].GetStringType());
            Assert.AreEqual(3, level.CurrentPlayer.GetInventory().Count);
            Assert.AreEqual(0, level.CurrentPlayer.GetAppliedInventory().Count);
            
            var afterStatistics = level.CurrentPlayer.GetStatistics();
            level.Board.IsInventory(inventoryPosition);
            
            Assert.AreEqual(beforeStatistics.Experience, afterStatistics.Experience);
            Assert.AreEqual(beforeStatistics.Force, afterStatistics.Force);
            Assert.AreEqual(beforeStatistics.Health - 3, afterStatistics.Health);
        }
    }
}