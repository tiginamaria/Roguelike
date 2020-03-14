using System;

namespace Roguelike
{
    public class Player : Character
    {
        public Player(Position startPosition) : base(startPosition)
        {
        }

        public virtual void Update(Board board)
        {
            var newPosition = Position.Add(IntentVerticalMove, IntentHorizontalMove);
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