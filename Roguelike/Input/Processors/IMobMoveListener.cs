using Roguelike.Model;
using Roguelike.Model.Mobs;

namespace Roguelike.Input.Processors
{
    /// <summary>
    /// Observes mob moves.
    /// </summary>
    public interface IMobMoveListener
    {
        /// <summary>
        /// This method is called when a mob intents to move to the given position.
        /// </summary>
        void Move(Mob mob, Position intentPosition);
    }
}