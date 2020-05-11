using Roguelike.Model;

namespace Roguelike.View
{
    public interface IPlayView
    {
        void Draw(Level level);
        void UpdatePosition(Level level, Position position);
        void UpdateMob(Level level, Position first, Position second);
        void UpdatePlayer(Level level, Position first, Position second);
        void UpdateInventory(Level level);
    }
}
