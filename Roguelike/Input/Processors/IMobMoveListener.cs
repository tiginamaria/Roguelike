using Roguelike.Model;
using Roguelike.Model.Mobs;

namespace Roguelike.Input.Processors
{
    /// <summary>
    /// Observes mob moves.
    /// </summary>
    public interface IMobMoveListener
    {
        void Move(Mob mob, Position intentPosition);
    }
}