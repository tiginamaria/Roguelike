using System;
    
namespace Roguelike
{
    public class Level
    {
        public Player Player 
        { get; }
        public Board Board
        { get; }

        public Level(Board board)
        {
            Board = board;
            Player = FindPlayer(board);
        }

        private Player FindPlayer(Board board)
        {
            for (var row = 0; row < board.Height; row++)
            {
                for (var col = 0; col < board.Width; col++)
                {
                    var position = new Position(row, col);
                    var gameObject = board.GetObject(position);
                    if (gameObject is Player)
                    {
                        return gameObject as Player;
                    }
                }
            }
            throw new Exception("Player not found.");
        }

        public void Update()
        {
            Player.Update(Board);
        }
    }
}