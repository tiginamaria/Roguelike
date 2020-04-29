using System.Collections.Generic;
using System.IO;
using System.Text;
using Roguelike.Initialization;
using Roguelike.Model.Inventory;
using Roguelike.Model.Mobs;
using Roguelike.Model.Objects;
using Roguelike.Model.PlayerModel;
using Roguelike.View;

namespace Roguelike.Model
{
    public class LevelSnapshot
    {
        private readonly Board board;

        public LevelSnapshot(Board board)
        {
            this.board = board;
        }

        public void Dump(string path)
        {
            using (var txtWriter = new StreamWriter(File.Open(path, FileMode.Create)))
            {
                var configurations = new List<string>();
                txtWriter.Write($"{board.Height} {board.Width}");
                txtWriter.WriteLine();
                for (var row = 0; row < board.Height; row++)
                {
                    for (var col = 0; col < board.Width; col++)
                    {
                        var position = new Position(row, col);
                        var gameObject = board.GetObject(position);

                        switch (gameObject)
                        {
                            case Mob mob:
                                configurations.Add(DumpMob(mob));
                                break;
                            case Player player:
                                configurations.Add(DumpPlayer(player));
                                break;
                            case InventoryItem inventory:
                                configurations.Add(DumpInventory(inventory));
                                break;
                        }

                        var c = board.IsWall(position) ? BoardObject.Wall : BoardObject.Empty;
                        txtWriter.Write($"{c} ");
                    }

                    txtWriter.WriteLine();
                }

                configurations.ForEach(s =>
                {
                    txtWriter.Write(s);
                    txtWriter.WriteLine();
                });
            }
        }
        private string DumpInventory(InventoryItem inventory)
        {
            var typeString = inventory.GetType();
            var positionString = $"{inventory.Position.Y} {inventory.Position.X}";
            return $"{typeString} {positionString}";
        }

        private string DumpMob(Mob mob)
        {
            var typeString = mob.GetType();
            var statistics = mob.GetStatistics();
            var positionString = $"{mob.Position.Y} {mob.Position.X}";
            var statisticsString = $"{statistics.Experience} {statistics.Force} {statistics.Health}";
            return $"{typeString} {positionString} {statisticsString}";
        }
        
        private string DumpPlayer(Player player)
        {
            StringBuilder sb = new StringBuilder();
            var typeString = player.GetType();
            var statistics = player.GetStatistics();
            var positionString = $"{player.Position.Y} {player.Position.X}";
            var statisticsString = $"{statistics.Experience} {statistics.Force} {statistics.Health}";
            var inventoryCount = $"{player.GetInventory().Count}";
            var appliedInventoryCount = $"{player.GetAppliedInventory().Count}";
            sb.Append($"{typeString} {positionString} {statisticsString} {inventoryCount} {appliedInventoryCount}");
            foreach (var inventoryItem in player.GetInventory())
            {
                sb.Append($" {DumpInventory(inventoryItem)}");
            }
            foreach (var appliedInventoryItem in player.GetAppliedInventory())
            {
                sb.Append($" {DumpInventory(appliedInventoryItem)}");
            }

            return sb.ToString();
        }
    }
}