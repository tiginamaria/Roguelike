using System;
using Roguelike.Model.Inventory;
using Roguelike.Model.Objects;

namespace Roguelike.Model
{
    /// <summary>
    /// Represents an abstract character that is able to move and fight.
    /// </summary>
    public abstract class Character : GameObject
    {
        /// <summary>
        /// An event that is risen when the characted dies.
        /// </summary>
        public event EventHandler OnDie;

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
                Fight(board.GetObject(newPosition) as Character);
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
        /// Fight with another character.
        /// </summary>
        public abstract void Fight(Character other);

        /// <summary>
        /// Adds the given item to the inventory.
        /// </summary>
        public abstract void Collect(InventoryItem inventory);

        /// <summary>
        /// Accept confusion.
        /// </summary>
        public abstract void BecomeConfused();

        protected Character(Position initPosition) : base(initPosition)
        {
        }


        public virtual void Die() => OnDie?.Invoke(this, EventArgs.Empty);
    }
}