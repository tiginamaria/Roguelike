namespace Roguelike.Model.Mobs
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMobBehaviour
    {
        /// <summary>
        /// Performs a move depending on a behaviour type.
        /// </summary>
        Position MakeMove(Level level, Position position);

        /// <summary>
        /// Returns a string representation if the behaviour.
        /// </summary>
        string GetStringType();
    }
}
