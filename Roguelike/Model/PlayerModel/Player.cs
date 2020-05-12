using System;
using System.Collections.Generic;
using System.Linq;
using Roguelike.Model.Inventory;

namespace Roguelike.Model.PlayerModel
{
    /// <summary>
    /// Represents a player.
    /// </summary>
    public class Player : AbstractPlayer
    {
        private readonly CharacterStatistics statistics;
        private readonly Level level;
        private readonly List<InventoryItem> inventoryItems;
        private readonly List<InventoryItem> appliedInventoryItems;
        public Player(string login, Level level, Position startPosition) 
            : this(login, level, startPosition, new CharacterStatistics(2, 15, 1),  
                new List<InventoryItem>(), new List<InventoryItem>())
        {
        }

        public Player(string login, Level level, Position startPosition, CharacterStatistics statistics,
            List<InventoryItem> inventoryItems, List<InventoryItem> appliedInventoryItems) : base(startPosition, login)
        {
            this.level = level;
            this.statistics = statistics;
            this.inventoryItems = inventoryItems;
            this.appliedInventoryItems = appliedInventoryItems;
        }

        public override CharacterStatistics GetStatistics()
        {
            return statistics;
        }

        public override void Confuse(Character other)
        {
            other.AcceptConfuse(this);
            statistics.Experience++;
            statistics.Force += other.GetStatistics().Force / 2;
        }

        public override void Collect(InventoryItem newInventory)
        {
            inventoryItems.Add(newInventory);
        }

        public override void AcceptConfuse(Character other)
        {
            statistics.Health = Math.Max(0, statistics.Health - other.GetStatistics().Force / 2);

            if (statistics.Health == 0)
            {
                level.DeletePlayer(this);
                Delete(level.Board);
                OnDie?.Invoke(this, EventArgs.Empty);
                return;
            }
            statistics.Experience = Math.Max(0, statistics.Experience - 1);
            var confusedPlayer = new ConfusedPlayer(level, this);
            level.UpdatePlayer(confusedPlayer);
            level.Board.SetObject(Position, confusedPlayer);
        }

        public override void PutOff(string inventoryType)
        {
            var inventoryToRemove = appliedInventoryItems.FirstOrDefault(inv => inv.GetStringType() == inventoryType);
            if (inventoryToRemove != null)
            {
                inventoryToRemove.Remove(statistics);
                appliedInventoryItems.Remove(inventoryToRemove);
            }
        }

        public override void PutOn(string inventoryType)
        {
            var inventoryToActivate = inventoryItems.FirstOrDefault(inv => inv.GetStringType() == inventoryType);
            if (inventoryToActivate != null)
            {
                appliedInventoryItems.Add(inventoryToActivate);
                inventoryToActivate.Apply(statistics);
                inventoryItems.Remove(inventoryToActivate);
            }
        }

        public override List<InventoryItem> GetInventory()
        {
            return inventoryItems;
        }

        public override List<InventoryItem> GetAppliedInventory()
        {
            return appliedInventoryItems;
        }

        public override string GetStringType()
        {
            return PlayerType.Player;
        }
    }
}
