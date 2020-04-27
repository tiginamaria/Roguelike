using System;

namespace Roguelike.Model.PlayerModel
{
    /// <summary>
    /// Represents a player.
    /// </summary>
    public class Player : AbstractPlayer
    {
        private readonly CharacterStatistics statistics;
        private readonly Level level;

        public Player(Level level, Position startPosition) : base(startPosition)
        {
            this.level = level;
            statistics = new CharacterStatistics(1, 15, 1);
        }

        public Player(Level level, Position startPosition, CharacterStatistics statistics) : base(startPosition)
        {
            this.level = level;
            this.statistics = statistics;
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
