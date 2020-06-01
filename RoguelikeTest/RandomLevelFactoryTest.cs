using System;
using System.Collections.Generic;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.Inventory;
using Roguelike.Model.Mobs;
using Roguelike.Model.Objects;
using Roguelike.Model.PlayerModel;

namespace RoguelikeTest
{
    public class RandomLevelFactoryTest
    {
        [Test]
        public void RandomLevelPlayersTest()
        {
            var factory = new RandomLevelFactory();
            factory.SetPlayerFactory(new NetworkPlayerFactory(new ExitGameInteractor()));
            var level = factory.CreateLevel();

            level.AddPlayerAtEmpty("player1");
            Assert.IsTrue(level.ContainsPlayer("player1"));
            var player1 = level.GetPlayer("player1");
            level.CurrentPlayer = player1;
            
            level.AddPlayerAtEmpty("player2");
            Assert.IsTrue(level.ContainsPlayer("player2"));
            var player2 = level.GetPlayer("player2");
            
            Assert.IsTrue(level.ContainsPlayer("player1"));
            Assert.IsFalse(level.ContainsPlayer("player3"));

            var player2Position = player2.Position; 
            level.DeletePlayer(player2);
            Assert.IsFalse(level.ContainsPlayer("player2"));
            
            var newPlayer1 = new Player("player1", level, player2Position);
            level.UpdatePlayer(newPlayer1);
            
            Assert.AreNotEqual(player1.Position, level.GetPlayer("player1").Position);
        }

        [Test]
        public void RandomLevelSetFactoriesTest()
        {
            var factory = new RandomLevelFactory();
            factory.SetMobFactory(new MobFactory());
            var level = new RandomLevelFactory().CreateLevel();
            var board = level.Board;
            for (var i = 0; i < board.Height; i++)
            {
                for (var j = 0; j < board.Width; j++)
                {
                    var position = new Position(i, j);
                    switch (board.GetObject(position))
                    {
                        case Mob mob:
                            Assert.IsTrue(level.Mobs.Contains(mob));
                            Assert.IsNotInstanceOf(typeof(ConfusedMobBehaviour), mob.GetBehaviour());
                            break;
                        default:
                            Assert.True(true);
                            break;
                    }
                }
            }
        }
        
        [Test]
        public void RandomNetworkLevelSetFactoriesTest()
        {
            var factory = new RandomLevelFactory();
            factory.SetMobFactory(new NetworkMobFactory());
            var level = factory.CreateLevel();
            var board = level.Board;
            for (var i = 0; i < board.Height; i++)
            {
                for (var j = 0; j < board.Width; j++)
                {
                    var position = new Position(i, j);
                    switch (board.GetObject(position))
                    {
                        case Mob mob:
                            Assert.IsTrue(level.Mobs.Contains(mob));
                            Assert.IsNotInstanceOf(typeof(ConfusedMobBehaviour), mob.GetBehaviour());
                            break;
                        default:
                            Assert.True(true);
                            break;
                    }
                }
            }
        }
        
        [Test]
        public void RandomLevelConfigurationTest()
        {
            var level = new RandomLevelFactory().CreateLevel();
            var board = level.Board;
            for (var i = 0; i < board.Height; i++)
            {
                for (var j = 0; j < board.Width; j++)
                {
                    var position = new Position(i, j);
                    switch (board.GetObject(position))
                    {
                        case Mob mob:
                            Assert.IsTrue(level.Mobs.Contains(mob));
                            break;
                        case EmptyCell _:
                            Assert.IsTrue(level.Board.IsEmpty(position));
                            break;
                        case Wall _:
                            Assert.IsTrue(level.Board.IsWall(position));
                            break;
                        case InventoryItem _:
                            Assert.IsTrue(level.Board.IsInventory(position));
                            break;
                    }
                }
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
        public void ConnectivityTest()
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
            
            Assert.IsFalse(labyrinth.AreConnected(-1, -1, 1, 1));
            Assert.IsFalse(labyrinth.AreConnected(1, 1, -1, -1));
        }
    }
}