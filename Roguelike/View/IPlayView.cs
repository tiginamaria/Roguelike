using Roguelike.Model;

namespace Roguelike.View
{
    public interface IPlayView
    {
        /// <summary>
        /// Redraws the given position.
        /// </summary>
        void UpdatePosition(Level level, Position position);
        
        /// <summary>
        /// Redraws two given position caused by mob.
        /// </summary>
        void UpdateMob(Level level, Position first, Position second);
        
        /// <summary>
        /// Redraws two given positions caused by player.
        /// </summary>
        void UpdatePlayer(Level level, Position first, Position second);
        
        /// <summary>
        /// Redraws player's inventory.
        /// </summary>
        void UpdateInventory(Level level);
    }
}
