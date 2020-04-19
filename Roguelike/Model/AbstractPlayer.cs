namespace Roguelike.Model
{
    public abstract class AbstractPlayer : GameObject, ICharacter
    {
        protected AbstractPlayer(Position startPosition) : base(startPosition)
        {
        }

        /// <summary>
        /// Performs a logic of the move intent.
        /// </summary>
        public abstract void Move(int intentVerticalMove, int intentHorizontalMove, Board board);

        protected bool CanMoveTo(Position newPosition, Board board)
        {
            return newPosition != Position && board.CheckOnBoard(newPosition) &&
                   !board.IsWall(newPosition);
        }

        public abstract CharacterStatistics GetStatistics();
        public abstract void Confuse(ICharacter other);
        public abstract void AcceptConfuse(ICharacter other);
    }
}