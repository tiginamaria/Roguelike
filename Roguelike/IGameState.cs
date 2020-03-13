namespace Roguelike
{
    public interface IGameState
    {
        void Update();
        void Draw();
        void InvokeState();
        void CloseState();
    }
}