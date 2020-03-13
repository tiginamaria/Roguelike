namespace Roguelike
{
    public abstract class GameObject
    {
        public Position Position { get; }

        public GameObject(Position initPosition)
        {
            Position = initPosition;
        }
    }
}