using System.Collections.Generic;
using System.IO;
using System.Text;
using Roguelike.Model.Inventory;
using Roguelike.Model.Mobs;
using Roguelike.Model.Objects;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Model
{
    public class LevelSnapshot
    {
        private readonly Board board;

        public LevelSnapshot(Board board)
        {
            this.board = board;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var configurations = new List<string>();
            sb.AppendLine($"{board.Height} {board.Width}");
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
                    sb.Append($"{c} ");
                }

                sb.AppendLine();
            }

            foreach (var element in configurations)
            {
                sb.AppendLine(element);
            }

            return sb.ToString();
        }

        public void Dump(string path)
        {
            using var txtWriter = new StreamWriter(File.Open(path, FileMode.Create));
            txtWriter.Write(ToString());
        }
        
        private string DumpInventory(InventoryItem inventory)
        {
            var typeString = inventory.GetStringType();
            var positionString = $"{inventory.Position.Y} {inventory.Position.X}";
            return $"{typeString} {positionString}";
        }

        private string DumpMob(Mob mob)
        {
            var typeString = mob.GetStringType();
            var statistics = mob.GetStatistics();
            var positionString = $"{mob.Position.Y} {mob.Position.X}";
            var statisticsString = $"{statistics.Experience} {statistics.Force} {statistics.Health}";
            return $"{typeString} {positionString} {statisticsString}";
        }
        
        private string DumpPlayer(Player player)
        {
            var sb = new StringBuilder();
            var typeString = player.GetStringType();
            var statistics = player.GetStatistics();
            var positionString = $"{player.Position.Y} {player.Position.X}";
            var statisticsString = $"{statistics.Experience} {statistics.Force} {statistics.Health}";
            sb.Append($"{typeString} {positionString} {statisticsString} ");
            
            sb.Append($"{player.GetInventory().Count} ");
            foreach (var inventoryItem in player.GetInventory())
            {
                sb.Append($" {DumpInventory(inventoryItem)}");
            }
            
            sb.Append($"{player.GetAppliedInventory().Count}");
            foreach (var appliedInventoryItem in player.GetAppliedInventory())
            {
                sb.Append($" {DumpInventory(appliedInventoryItem)}");
            }

            return sb.ToString();
        }
    }
}