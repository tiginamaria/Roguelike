namespace Roguelike.Input.Processors
{
    /// <summary>
    /// An interface to process ticks.
    /// </summary>
    public interface ITickProcessor
    {
        /// <summary>
        /// This method is called every tick.
        /// </summary>
        void ProcessTick();
    }
}
