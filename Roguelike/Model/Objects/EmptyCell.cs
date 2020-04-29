namespace Roguelike.Model.Objects
{
    /// <summary>
    /// Represents an Empty Cell
    /// </summary>
    public class EmptyCell : GameObject
    {
        public EmptyCell(Position initPosition) : base(initPosition)
        {
        }

        public override string GetStringType()
        {
            return BoardObject.Empty;
        }
    }
}
