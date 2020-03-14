namespace Roguelike
{
    public abstract class GameObject
    {
        public Position Position { get; protected set; }

        public GameObject(Position initPosition)
        {
            Position = initPosition;
        }
    }
}