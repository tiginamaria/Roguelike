using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Roguelike.Initialization;
using Roguelike.Model;
using Roguelike.Model.Inventory;
using Roguelike.Model.Mobs;

namespace RoguelikeTest
{
    public class FileLevelFactoryTest
    {
        private int height;
        private int width;
        private char[][] boardConfiguration;

        [SetUp]
        public void SetUp()
        {
            height = 5;
            width = 6;
            boardConfiguration = new[]
            {
                new[] {'#', '@', '#', '#', '#', '.'},
                new[] {'#', '.', 'H', '#', '.', '%'},
                new[] {'#', '*', '#', '.', '$', '#'},
                new[] {'#', 'E', '#', 'A', '.', '#'},
                new[] {'#', '#', 'o', '.', 'F', '#'}
            };
        }

        private void CheckLevelConfiguration(Level level)
        {
            Assert.AreEqual(height, level.Board.Height);
            Assert.AreEqual(width, level.Board.Width);


            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var position = new Position(i, j);
                    switch (boardConfiguration[i][j])
                    {
                        case '#':
                            Assert.AreEqual("#", level.Board.GetObject(position).GetStringType());
                            Assert.IsTrue(level.Board.IsWall(position));
                            break;
                        case '.':
                            Assert.AreEqual(".", level.Board.GetObject(position).GetStringType());
                            Assert.IsTrue(level.Board.IsEmpty(position));
                            break;
                        case '$':
                            Assert.IsTrue(level.ContainsPlayer("testplayer"));
                            var player = level.GetPlayer("testplayer");
                            level.CurrentPlayer = level.GetPlayer("testplayer");
                            Assert.IsTrue(level.IsCurrentPlayer(player));
                            Assert.AreEqual("$", level.Board.GetObject(position).GetStringType());
                            Assert.AreEqual(player.Position, position);
                            Assert.AreEqual(6, player.GetStatistics().Experience);
                            Assert.AreEqual(4, player.GetStatistics().Force);
                            Assert.AreEqual(5, player.GetStatistics().Health);
                            var inventory = player.GetInventory();
                            Assert.AreEqual(2, inventory.Count);
                            Assert.AreEqual(typeof(IncreaseAllItem), inventory[0].GetType());
                            Assert.AreEqual(typeof(IncreaseForceItem), inventory[1].GetType());
                            var appliedInventory = player.GetAppliedInventory();
                            Assert.AreEqual(1, appliedInventory.Count);
                            Assert.AreEqual(typeof(IncreaseHealthItem), appliedInventory[0].GetType());
                            break;
                        case '*':
                            Assert.AreEqual("*", level.Board.GetObject(position).GetStringType());
                            var aggressiveMob = level.Board.GetObject(position) as Mob;
                            Assert.IsNotNull(aggressiveMob);
                            Assert.AreEqual(typeof(AggressiveMobBehaviour), aggressiveMob.GetBehaviour().GetType());
                            Assert.AreEqual(1, aggressiveMob.GetStatistics().Experience);
                            Assert.AreEqual(1, aggressiveMob.GetStatistics().Force);
                            Assert.AreEqual(11, aggressiveMob.GetStatistics().Health);
                            break;
                        case '@':
                            Assert.AreEqual("@", level.Board.GetObject(position).GetStringType());
                            var passiveMob = level.Board.GetObject(position) as Mob;
                            Assert.IsNotNull(passiveMob);
                            Assert.AreEqual(typeof(PassiveMobBehaviour), passiveMob.GetBehaviour().GetType());
                            Assert.AreEqual(0, passiveMob.GetStatistics().Experience);
                            Assert.AreEqual(2, passiveMob.GetStatistics().Force);
                            Assert.AreEqual(1, passiveMob.GetStatistics().Health);
                            break;
                        case '%':
                            Assert.AreEqual("%", level.Board.GetObject(position).GetStringType());
                            var cowardMob = level.Board.GetObject(position) as Mob;
                            Assert.IsNotNull(cowardMob);
                            Assert.AreEqual(typeof(CowardMobBehaviour), cowardMob.GetBehaviour().GetType());
                            Assert.AreEqual(1, cowardMob.GetStatistics().Experience);
                            Assert.AreEqual(3, cowardMob.GetStatistics().Force);
                            Assert.AreEqual(2, cowardMob.GetStatistics().Health);
                            break;
                        case 'o':
                            Assert.AreEqual("o", level.Board.GetObject(position).GetStringType());
                            var confusedMob = level.Board.GetObject(new Position(i, j)) as Mob;
                            Assert.IsNotNull(confusedMob);
                            Assert.AreEqual(typeof(ConfusedMobBehaviour), confusedMob.GetBehaviour().GetType());
                            Assert.AreEqual(6, confusedMob.GetStatistics().Experience);
                            Assert.AreEqual(3, confusedMob.GetStatistics().Force);
                            Assert.AreEqual(2, confusedMob.GetStatistics().Health);
                            break;
                        case 'F':
                            Assert.AreEqual("F", level.Board.GetObject(position).GetStringType());
                            var forceInventory = level.Board.GetObject(new Position(i, j)) as InventoryItem;
                            Assert.IsNotNull(forceInventory);
                            Assert.AreEqual(typeof(IncreaseForceItem), forceInventory.GetType());
                            break;
                        case 'H':
                            Assert.AreEqual("H", level.Board.GetObject(position).GetStringType());
                            var healthInventory = level.Board.GetObject(new Position(i, j)) as InventoryItem;
                            Assert.IsNotNull(healthInventory);
                            Assert.AreEqual(typeof(IncreaseHealthItem), healthInventory.GetType());
                            break;
                        case 'E':
                            Assert.AreEqual("E", level.Board.GetObject(position).GetStringType());
                            var experienceInventory = level.Board.GetObject(new Position(i, j)) as InventoryItem;
                            Assert.IsNotNull(experienceInventory);
                            Assert.AreEqual(typeof(IncreaseExperienceItem), experienceInventory.GetType());
                            break;
                        case 'A':
                            Assert.AreEqual("A", level.Board.GetObject(position).GetStringType());
                            var allInventory = level.Board.GetObject(new Position(i, j)) as InventoryItem;
                            Assert.IsNotNull(allInventory);
                            Assert.AreEqual(typeof(IncreaseAllItem), allInventory.GetType());
                            break;
                    }
                }
            }
        }

        [Test]
        public void LevelSnapshotStringTest()
        {
            var stringSnapshot = "5 6\n" +
                                 "# . # # # .\n" +
                                 "# . . # . .\n" +
                                 "# . # . . #\n" +
                                 "# . # . . #\n" +
                                 "# # . . . #\n" +
                                 "@ 0 1 2 1 0\n" +
                                 "H 1 2\n" +
                                 "% 1 5 3 2 1\n" +
                                 "* 2 1 1 11 1\n" +
                                 "$ testplayer 2 4 4 5 6 2 A 0 0 F 1 1 1 H 2 2\n" +
                                 "E 3 1\n" +
                                 "A 3 3\n" +
                                 "o 4 2 3 2 6\n" +
                                 "F 4 4";
            var levelFromString = FileLevelFactory.FromString(stringSnapshot).CreateLevel();
            CheckLevelConfiguration(levelFromString);
        }

        [Test]
        public void LevelFromSnapshotFileTest()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "../../../test_maps/level_snapshot.txt");
            var level = new FileLevelFactory(path).CreateLevel();
            CheckLevelConfiguration(level);
        }

        [Test]
        public void LevelToSnapshotFileTest()
        {
            var stringSnapshot = "5 6\n" +
                                 "# . # # # . \n" +
                                 "# . . # . . \n" +
                                 "# . # . . # \n" +
                                 "# . # . . # \n" +
                                 "# # . . . # \n" +
                                 "@ 0 1 2 1 0\n" +
                                 "H 1 2\n" +
                                 "% 1 5 3 2 1\n" +
                                 "* 2 1 1 11 1\n" +
                                 "$ testplayer 2 4 4 5 6 2 A 0 0 F 1 1 1 H 2 2 \n" +
                                 "E 3 1\n" +
                                 "A 3 3\n" +
                                 "o 4 2 3 2 6\n" +
                                 "F 4 4\n";
            var level = FileLevelFactory.FromString(stringSnapshot).CreateLevel();
            level.CurrentPlayer = level.GetPlayer("testplayer");
            var levelSnapshot = level.Save();
            Assert.AreEqual(
                stringSnapshot.Split().Select(s => s.Trim()).Where(s => s.Length > 0).ToArray(),
                levelSnapshot.ToString().Split().Select(s => s.Trim()).Where(s => s.Length > 0).ToArray()
            );
        }

        [Test]
        public void BoardConfigurationTest()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "../../../test_maps/level_snapshot.txt");
            var level = new FileLevelFactory(path).CreateLevel();
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

        [Test]
        public void GraphConfigurationTest()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "../../../test_maps/level_snapshot.txt");
            var level = new FileLevelFactory(path).CreateLevel();
            var graph = level.Graph;
            var playerPosition = level.GetPlayer("testplayer").Position;
            Assert.AreEqual(4, level.Mobs.Count);
            var modPosition = level.GetMob(level.Mobs[0].Id.ToString()).Position;
            Assert.AreEqual(new Position(2, 4), graph.Farthest(playerPosition, modPosition));

            var inventoryPosition = new Position(4, 4);
            Assert.AreEqual(new Position(3, 4), graph.Nearest(playerPosition, inventoryPosition));
        }
    }
}