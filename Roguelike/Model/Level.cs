using System;
using System.Collections.Generic;
using Roguelike.Model.Mobs;
using Roguelike.Model.Objects;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Model
{
    /// <summary>
    /// Stores the information about the game level.
    /// </summary>
    public class Level
    {
        //public AbstractPlayer Player { get; set; }
        private readonly Dictionary<string, AbstractPlayer> players = new Dictionary<string, AbstractPlayer>();
        public List<Mob> Mobs { get; }
        public Board Board { get; }
        public BoardGraph Graph { get; }
        public AbstractPlayer CurrentPlayer { get; set; }

        public Level(Func<Level, Board> boardCreator)
        {
            Board = boardCreator(this);
            Mobs = Board.FindMobs();
            Graph = new BoardGraph(Board);
            var playersList = Board.FindPlayers();
            foreach (var player in playersList)
            {
                players.Add(player.Login, player);
            }
        }

        public LevelSnapshot Save()
        {
            return new LevelSnapshot(new Board(Board.Width, Board.Height, Board.CloneGameObjects()));
        }

        public Player AddPlayer(string login)
        {
            //TODO
            var newPlayer = new Player(login, this, new Position(1, 1));
            players.Add(login, newPlayer);
            Board.SetObject(new Position(1, 1), newPlayer);
            return newPlayer;
        }

        public Character GetCharacter(string login)
        {
            return players[login];
        }

        public Mob GetMob(string id)
        {
            return Mobs.Find(mob => mob.Id == int.Parse(id));
        }

        public bool IsCurrentPlayer(Character character)
        {
            if (!(character is AbstractPlayer))
            {
                return false;
            }

            var playerCharacter = (AbstractPlayer) character;
            return playerCharacter.Login == CurrentPlayer.Login;
        }

        public void UpdatePlayer(AbstractPlayer newPlayer)
        {
            players[newPlayer.Login] = newPlayer;
        }
    }
}
