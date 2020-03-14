namespace Roguelike
{
    public interface IGameState
    {
        void Update();
        void InvokeState();
    }
}