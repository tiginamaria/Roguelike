using System;

namespace Roguelike.Model
{
    /// <summary>
    /// Stores the information about the game level.
    /// </summary>
    public class Level
    {
        public AbstractPlayer Player { get; set; }
        public Board Board { get; }

        public Level(Func<Level, Board> boardCreator)
        {
            Board = boardCreator(this);
            Player = Board.FindPlayer();
        }
    }
}