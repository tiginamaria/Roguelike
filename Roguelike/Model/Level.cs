namespace Roguelike.Model
{
    /// <summary>
    /// Stores the information about the game level.
    /// </summary>
    public class Level
    {
        public Player Player { get; }
        public Board Board { get; }

        public Level(Board board)
        {
            Board = board;
            Player = board.FindPlayer();
        }
    }
}