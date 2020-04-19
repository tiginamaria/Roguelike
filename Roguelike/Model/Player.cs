namespace Roguelike.Model
{
    /// <summary>
    /// Represents a player.
    /// </summary>
    public class Player : AbstractPlayer
    {
        private readonly CharacterStatistics statistics = new CharacterStatistics(1, 1, 1);
        private readonly Level level;

        public Player(Level level, Position startPosition) : base(startPosition)
        {
            this.level = level;
        }

        /// <summary>
        /// Performs a logic of the move intent.
        /// </summary>
        public override void Move(int intentVerticalMove, int intentHorizontalMove, Board board)
        {
            var newPosition = Position.Add(intentVerticalMove, intentHorizontalMove);
            if (!CanMoveTo(newPosition, board))
            {
                return;
            }

            if (!board.IsWall(newPosition))
            {
                board.MoveObject(Position, newPosition);
            }

            Position = newPosition;
        }

        public override CharacterStatistics GetStatistics()
        {
            return statistics;
        }

        public override void Confuse(ICharacter other)
        {
            other.AcceptConfuse(this);
            statistics.Experience++;
            statistics.Force += other.GetStatistics().Force / 2;
        }

        public override void AcceptConfuse(ICharacter other)
        {
            statistics.Health -= other?.GetStatistics().Force / 2 ?? 0;
            statistics.Experience--;
            level.Player = new ConfusedPlayer(level, this);
        }
    }
}