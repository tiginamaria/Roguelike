namespace Roguelike.Model.Mobs
{
    public interface IMobBehaviour
    {
        Position MakeMove(Level level, Position position);

        string GetStringType();
    }
}
