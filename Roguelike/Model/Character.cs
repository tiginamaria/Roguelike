namespace Roguelike.Model
{
    public abstract class Character : GameObject
    {
        public abstract CharacterStatistics GetStatistics();

        private bool CanMoveTo(Position newPosition, Board board)
        {
            return newPosition != Position && board.CheckOnBoard(newPosition) &&
                   !board.IsWall(newPosition);
        }
        
        public virtual void Move(int intentVerticalMove, int intentHorizontalMove, Board board)
        {
            var newPosition = Position + new Position(intentVerticalMove, intentHorizontalMove);
            if (!CanMoveTo(newPosition, board))
            {
                return;
            } 
            
            if (board.IsCharacter(newPosition))
            {
                Confuse(board.GetObject(newPosition) as Character);
            }
            if (board.IsEmpty(newPosition))
            {
                board.MoveObject(Position, newPosition);
            }
            
            Position = newPosition;
        }
        public abstract void Confuse(Character other);
        public abstract void AcceptConfuse(Character other);

        protected Character(Position initPosition) : base(initPosition)
        {
        }
    }
}