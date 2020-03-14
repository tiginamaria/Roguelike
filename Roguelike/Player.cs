namespace Roguelike
{
    public class Player : GameObject
    {
        public Player(Position startPosition) : base(startPosition)
        {
        }

        public virtual void Move(int intentVerticalMove, int intentHorizontalMove, Board board)
        {
            var newPosition = Position.Add(intentVerticalMove, intentHorizontalMove);
            if (!CanMoveTo(newPosition, board))
            {
                return;
            }

            if (board.IsEmpty(newPosition))
            {
                board.MoveObject(Position, newPosition);
            }

            Position = newPosition;
        }

        private bool CanMoveTo(Position newPosition, Board board)
        {
            return newPosition != Position && board.CheckOnBoard(newPosition) &&
                   !board.IsWall(newPosition);
        }
    }
}