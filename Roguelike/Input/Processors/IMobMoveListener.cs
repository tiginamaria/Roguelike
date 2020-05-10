using Roguelike.Model;
using Roguelike.Model.Mobs;

namespace Roguelike.Input.Processors
{
    public interface IMobMoveListener
    {
        void Move(Mob mob, Position intentPosition);
    }
}