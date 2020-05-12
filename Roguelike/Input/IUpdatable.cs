namespace Roguelike.Input
{
    /// <summary>
    /// An interface for objects than should be automatically updated.
    /// </summary>
    public interface IUpdatable
    {
        void Update();
        void Stop();
    }
}
