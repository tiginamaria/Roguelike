using System;
using System.Collections.Generic;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Model;
using Roguelike.Model.Mobs;

 namespace TestRoguelike
{
    [TestFixture]
    public class LevelConfigurationTests
    {
        [Test]
        public void LevelGenerationTest()
        {
            const int height = 3;
            const int width = 6;
            var boardConfiguration = new[] {
                new[]{'#', '@', '#', '#', '#', '.'}, 
                new[]{'#', '#', '.', '#', '#', '%'}, 
                new[]{'#', '*', '#', '.', '$', '#'}
            };
            var path = TestUtils.WriteToFile(boardConfiguration, "level_test_map.txt");
            var level = new FileLevelFactory(path).CreateLevel();
            Assert.AreEqual(height, level.Board.Height);
            Assert.AreEqual(width, level.Board.Width);
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
                            break;
                        case '*':
                            var aggressiveMob = level.Board.GetObject(new Position(i, j)) as Mob;
                            Assert.IsNotNull(aggressiveMob);
                            Assert.AreEqual(typeof(AggressiveMobBehaviour), aggressiveMob.Behaviour.GetType());
                            break;
                        case '@':
                            var passiveMob = level.Board.GetObject(new Position(i, j)) as Mob;
                            Assert.IsNotNull(passiveMob);
                            Assert.AreEqual(typeof(PassiveMobBehaviour), passiveMob.Behaviour.GetType());
                            break;
                        case '%':
                            var cowardMob = level.Board.GetObject(new Position(i, j)) as Mob;
                            Assert.IsNotNull(cowardMob);
                            Assert.AreEqual(typeof(CowardMobBehaviour), cowardMob.Behaviour.GetType());
                            break;
                    }
                }
            }
        }

        [Test]
        public void BoardConfigurationTest()
        {
            const int height = 3;
            const int width = 6;
            var boardConfiguration = new[] {
                new[]{'#', '@', '#', '#', '#', '.'}, 
                new[]{'#', '#', '.', '#', '#', '%'}, 
                new[]{'#', '*', '#', '.', '$', '#'}
            };
            var path = TestUtils.WriteToFile(boardConfiguration, "level_test_map.txt");
            var board = new FileLevelFactory(path).CreateLevel().Board;
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
                        var transConnectedCells = GetConnectedCells(height, width, labyrinth, new Tuple<int, int>(i, j));
                    }
                }
            }
        }
    }
}
