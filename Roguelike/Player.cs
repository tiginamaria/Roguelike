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
            if (newPosition == Position || board.IsWall(newPosition))
            {
                return;
            }

            if (board.IsEmpty(newPosition))
            {
                board.MoveObject(Position, newPosition);
            }

            Position = newPosition;
        }
    }
}