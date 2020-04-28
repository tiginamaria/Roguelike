using System;
using System.Collections.Generic;
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
        private readonly List<InventoryItem> inventory;

        public Player(Level level, Position startPosition) : base(startPosition)
        {
            this.level = level;
            statistics = new CharacterStatistics(1, 15, 1);
            inventory = new List<InventoryItem>();
        }

        public Player(Level level, Position startPosition, CharacterStatistics statistics, List<InventoryItem> inventory) : base(startPosition)
        {
            this.level = level;
            this.statistics = statistics;
            this.inventory = inventory;
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
    }
}
