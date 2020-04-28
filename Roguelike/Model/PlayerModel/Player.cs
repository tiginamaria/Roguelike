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
        private readonly List<InventoryItem> appliedInventoryItems = new List<InventoryItem>();
        public Player(Level level, Position startPosition) : base(startPosition)
        {
            this.level = level;
            statistics = new CharacterStatistics(1, 15, 1);
            inventoryItems = new List<InventoryItem>();
        }

        public Player(Level level, Position startPosition, CharacterStatistics statistics, List<InventoryItem> inventoryItems) : base(startPosition)
        {
            this.level = level;
            this.statistics = statistics;
            this.inventoryItems = inventoryItems;
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
            statistics.Health -= other?.GetStatistics().Force / 2 ?? 0;

            if (statistics.Health <= 0)
            {
                OnDie?.Invoke(this, EventArgs.Empty);
                return;
            }
            statistics.Experience--;
            level.Player = new ConfusedPlayer(level, this);
            level.Board.SetObject(Position, level.Player);
        }

        public override void PutOff(string inventoryType)
        {
            var inventoryToRemove = appliedInventoryItems.FirstOrDefault(inv => inv.GetType() == inventoryType);
            if (inventoryToRemove != null)
            {
                inventoryToRemove.Remove(statistics);
                appliedInventoryItems.Remove(inventoryToRemove);
            }
        }

        public override void PutOn(string inventoryType)
        {
            var inventoryToActivate = inventoryItems.FirstOrDefault(inv => inv.GetType() == inventoryType);
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

        public override string GetType()
        {
            return PlayerType.Player;
        }
    }
}
