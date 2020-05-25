using Roguelike.Model.Inventory;
using Roguelike.Model.Objects;

namespace Roguelike.Model
{
    /// <summary>
    /// Represents an abstract character that is able to move and fight.
    /// </summary>
    public abstract class Character : GameObject
    {
        public abstract CharacterStatistics GetStatistics();

        private bool CanMoveTo(Position newPosition, Board board) =>
            newPosition != Position && board.CheckOnBoard(newPosition) &&
            !board.IsWall(newPosition);

        /// <summary>
        /// Attempts to move a character considering all its effects.
        /// </summary>
        public virtual void Move(int intentVerticalMove, int intentHorizontalMove, Board board) => 
            MoveStraightforward(intentVerticalMove, intentHorizontalMove, board);

        /// <summary>
        /// Attempts to move the character directly by the given delta.
        /// </summary>
        public virtual void MoveStraightforward(int intentVerticalMove, int intentHorizontalMove, Board board)
        {
            var newPosition = Position + new Position(intentVerticalMove, intentHorizontalMove);
            if (!CanMoveTo(newPosition, board))
            {
                return;
            }

            if (board.IsCharacter(newPosition))
            {
                MakeDamage(board.GetObject(newPosition) as Character);
            }

            if (board.IsInventory(newPosition))
            {
                Collect(board.GetObject(newPosition) as InventoryItem);
                board.MoveObject(Position, newPosition);
                Position = newPosition;
            }

            if (board.IsEmpty(newPosition))
            {
                board.MoveObject(Position, newPosition);
                Position = newPosition;
            }
        }

        /// <summary>
        /// Deletes itself from board.
        /// </summary>
        public void Delete(Board board) => board.DeleteObject(Position);

        /// <summary>
        /// Attacks another character. Calls AcceptDamage on it.
        /// </summary>
        public abstract void MakeDamage(Character other);
        
        /// <summary>
        /// Adds the given item to the inventory.
        /// </summary>
        public abstract void Collect(InventoryItem inventory);

        /// <summary>
        /// Updates itself when attacked by another character.
        /// </summary>
        public abstract void AcceptDamage(Character other);

        protected Character(Position initPosition) : base(initPosition)
        {
        }
    }
}
