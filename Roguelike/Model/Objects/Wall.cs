namespace Roguelike.Model.Objects
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a wall on the game board.
    /// </summary>
    public class Wall : GameObject
    {
        public Wall(Position position) : base(position)
        {
        }
        public override string GetType()
        {
            return BoardObject.Wall;
        }
    }
}
