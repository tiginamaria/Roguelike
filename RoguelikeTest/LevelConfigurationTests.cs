using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Model;
using Roguelike.Model.Inventory;
using Roguelike.Model.Mobs;

 namespace TestRoguelike
{
    [TestFixture]
    public class LevelConfigurationTests
    {
        private int height;
        private int width;
        private Level level;
        private char[][] boardConfiguration;
        
        [SetUp]
        public void SetUp()
        {
            height = 5;
            width = 6;
            boardConfiguration = new[] {
                new[]{'#', '@', '#', '#', '#', '.'}, 
                new[]{'#', '.', 'H', '#', '.', '%'}, 
                new[]{'#', '*', '#', '.', '$', '#'},
                new[]{'#', 'E', '#', 'A', '.', '#'},
                new[]{'#', '#', 'o', '.', 'F', '#'}
            };
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../test_maps/level_snapshot.txt");
            level = new FileLevelFactory(path).CreateLevel();
        }

        [Test]
        public void LevelSnapshotTest()
        {

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    switch (boardConfiguration[i][j])
                    {
                        case '#':
                            Assert.IsTrue(level.Board.IsWall(new Position(i, j)));
                            break;
                        case '.':
                            Assert.IsTrue(level.Board.IsEmpty(new Position(i, j)));
                            break;
                        case '$':
                            Assert.AreEqual(level.Player.Position, new Position(i, j));
                            Assert.AreEqual(6, level.Player.GetStatistics().Experience);
                            Assert.AreEqual(4, level.Player.GetStatistics().Force);
                            Assert.AreEqual(5, level.Player.GetStatistics().Health);
                            var inventory = level.Player.GetInventory();
                            Assert.AreEqual(2, inventory.Count);
                            Assert.AreEqual(typeof(IncreaseAllItem), inventory[0].GetType());
                            Assert.AreEqual(typeof(IncreaseForceItem), inventory[1].GetType());
                            var appliedInventory = level.Player.GetAppliedInventory();
                            Assert.AreEqual(1, appliedInventory.Count);
                            Assert.AreEqual(typeof(IncreaseHealthItem), appliedInventory[0].GetType());
                            break;
                        case '*':
                            var aggressiveMob = level.Board.GetObject(new Position(i, j)) as Mob;
                            Assert.IsNotNull(aggressiveMob);
                            Assert.AreEqual(typeof(AggressiveMobBehaviour), aggressiveMob.Behaviour.GetType());
                            Assert.AreEqual(1, aggressiveMob.GetStatistics().Experience);
                            Assert.AreEqual(1, aggressiveMob.GetStatistics().Force);
                            Assert.AreEqual(11, aggressiveMob.GetStatistics().Health);
                            break;
                        case '@':
                            var passiveMob = level.Board.GetObject(new Position(i, j)) as Mob;
                            Assert.IsNotNull(passiveMob);
                            Assert.AreEqual(typeof(PassiveMobBehaviour), passiveMob.Behaviour.GetType());
                            Assert.AreEqual(0, passiveMob.GetStatistics().Experience);
                            Assert.AreEqual(2, passiveMob.GetStatistics().Force);
                            Assert.AreEqual(1, passiveMob.GetStatistics().Health);
                            break;
                        case '%':
                            var cowardMob = level.Board.GetObject(new Position(i, j)) as Mob;
                            Assert.IsNotNull(cowardMob);
                            Assert.AreEqual(typeof(CowardMobBehaviour), cowardMob.Behaviour.GetType());
                            Assert.AreEqual(1, cowardMob.GetStatistics().Experience);
                            Assert.AreEqual(3, cowardMob.GetStatistics().Force);
                            Assert.AreEqual(2, cowardMob.GetStatistics().Health);
                            break;
                        case 'o':
                            var confusedMob = level.Board.GetObject(new Position(i, j)) as Mob;
                            Assert.IsNotNull(confusedMob);
                            Assert.AreEqual(typeof(ConfusedMobBehaviour), confusedMob.Behaviour.GetType());
                            Assert.AreEqual(6, confusedMob.GetStatistics().Experience);
                            Assert.AreEqual(3, confusedMob.GetStatistics().Force);
                            Assert.AreEqual(2, confusedMob.GetStatistics().Health);
                            break;
                        case 'F':
                            var forceInventory = level.Board.GetObject(new Position(i, j)) as InventoryItem;
                            Assert.IsNotNull(forceInventory);
                            Assert.AreEqual(typeof(IncreaseForceItem), forceInventory.GetType());
                            break;
                        case 'H':
                            var healthInventory = level.Board.GetObject(new Position(i, j)) as InventoryItem;
                            Assert.IsNotNull(healthInventory);
                            Assert.AreEqual(typeof(IncreaseHealthItem), healthInventory.GetType());
                            break;
                        case 'E':
                            var experienceInventory = level.Board.GetObject(new Position(i, j)) as InventoryItem;
                            Assert.IsNotNull(experienceInventory);
                            Assert.AreEqual(typeof(IncreaseExperienceItem), experienceInventory.GetType());
                            break;
                        case 'A':
                            var allInventory = level.Board.GetObject(new Position(i, j)) as InventoryItem;
                            Assert.IsNotNull(allInventory);
                            Assert.AreEqual(typeof(IncreaseAllItem), allInventory.GetType());
                            break;
                    }
                }
            }
            
        }

        [Test]
        public void BoardConfigurationTest()
        {
            var board = level.Board;
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Assert.IsTrue(board.CheckOnBoard(new Position(i, j)));
                }
            }
            for (var i = 0; i < height; i++)
            {
                Assert.IsFalse(board.CheckOnBoard(new Position(i, -1)));
                Assert.IsFalse(board.CheckOnBoard(new Position(i, width)));
            }
            for (var j = 0; j < width; j++)
            {
                Assert.IsFalse(board.CheckOnBoard(new Position(-1, j)));
                Assert.IsFalse(board.CheckOnBoard(new Position(height, j)));
            }
        }

        private HashSet<Tuple<int, int>> GetConnectedCells(int height, int width, Labyrinth labyrinth, Tuple<int, int> cell)
        {
            var connectedCells = new HashSet<Tuple<int, int>>();
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (labyrinth.AreConnected(i, j, cell.Item1, cell.Item2))
                    {
                        connectedCells.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            return connectedCells;
        }
        
        [Test]
        public void LabyrinthValidCellTest()
        {
            const int height = 3;
            const int width = 6;
            var labyrinth = new Labyrinth(height, width);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Assert.IsTrue(labyrinth.IsValidCell(i, j));
                }
            }
            for (var i = 0; i < height; i++)
            {
                Assert.IsFalse(labyrinth.IsValidCell(i, -1));
                Assert.IsFalse(labyrinth.IsValidCell(i, width));
            }
            for (var j = 0; j < width; j++)
            {
                Assert.IsFalse(labyrinth.IsValidCell(-1, j));
                Assert.IsFalse(labyrinth.IsValidCell(height, j));
            }
        }
        
        [Test]
        public void TransConnectivityTest()
        {
            const int height = 3;
            const int width = 4;
            var labyrinth = new Labyrinth(height, width);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var connectedCells = GetConnectedCells(height, width, labyrinth, new Tuple<int, int>(i, j));
                    foreach (var connectedCell in connectedCells)
                    {
                        Assert.IsTrue(labyrinth.AreConnected(i, j, connectedCell.Item1, connectedCell.Item2));
                    }
                }
            }
        }
    }
}
