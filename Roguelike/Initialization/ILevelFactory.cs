using Roguelike.Model;

namespace Roguelike.Initialization
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
