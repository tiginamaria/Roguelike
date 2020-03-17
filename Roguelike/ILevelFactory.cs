namespace Roguelike
{
    /// <summary>
    /// Interface for level factories. 
    /// </summary>
    public interface ILevelFactory
    {
        /// <summary>
        /// Returns a new level.
        /// </summary>
        Level CreateLevel();
    }
}