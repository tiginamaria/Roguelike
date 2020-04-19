namespace Roguelike.Model
{
    public interface IMobBehaviour
    {
        Position MakeMove(Level level, Position position);
    }
}