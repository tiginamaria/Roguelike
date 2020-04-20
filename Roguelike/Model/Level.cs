using System;
using System.Collections.Generic;

namespace Roguelike.Model
{
    /// <summary>
    /// Stores the information about the game level.
    /// </summary>
    public class Level
    {
        public AbstractPlayer Player { get; set; }
        public List<Mob> Mobs { get; }
        public Board Board { get; }
        public BoardGraph Graph { get; }

        public Level(Func<Level, Board> boardCreator)
        {
            Board = boardCreator(this);
            Player = Board.FindPlayer();
            Mobs = Board.FindMobs();
            Graph = new BoardGraph(Board);
        }
    }
}