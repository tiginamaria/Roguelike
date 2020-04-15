namespace Roguelike.Initialization
{
    /// <summary>
    /// Interface for game states.
    /// </summary>
    public interface IGameState
    {
        /// <summary>
        /// This method is called automatically when state is invoked.
        /// </summary>
        void InvokeState();
    }
}
