namespace Roguelike.Model
{
    /// <summary>
    /// Base class for all game objects on the game board.
    /// </summary>
    public abstract class GameObject
    {
        public Position Position { get; protected set; }

        public GameObject(Position initPosition)
        {
            Position = initPosition;
        }
    }
}
