namespace Roguelike.Input
{
    /// <summary>
    /// An interface for objects than should be automatically updated.
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// This method is called automatically to update the object.
        /// </summary>
        void Update();
        
        /// <summary>
        /// This method is called to stop object updating.
        /// </summary>
        void Stop();
    }
}
