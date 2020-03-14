namespace Roguelike
{
    public class Player : Character
    {
        public Player(Position startPosition) : base(startPosition)
        {
        }

        public override void Update(Board board)
        {
            var newPosition = Position.Add(IntentVerticalMove, IntentVerticalMove);
            if (newPosition == Position || board.IsWall(newPosition))
            {
                return;
            }

            if (board.IsEmpty(newPosition))
            {
                board.MoveObject(Position, newPosition);
            }

            Position = newPosition;
            ClearMoveIntent();
        }
    }
}