namespace Roguelike
{
    public abstract class GameObject
    {
        public Position Position { get; }

        public GameObject(int startY, int startX)
        {
            Position = new Position(startY, startX);
        }
        
        public void Update()
        {}
    }
}