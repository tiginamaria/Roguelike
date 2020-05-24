using Roguelike.Model;

namespace Roguelike.View
{
    /// <summary>
    /// Displays nothing. Used for the server mode.
    /// </summary>
    public class VoidView : IPlayView
    {
        public void UpdatePosition(Level level, Position position)
        {
        }

        public void UpdateMob(Level level, Position first, Position second)
        {
        }

        public void UpdatePlayer(Level level, Position first, Position second)
        {
        }

        public void UpdateInventory(Level level)
        {
        }
    }
}