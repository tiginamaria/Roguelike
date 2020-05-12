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

        public AbstractPlayer GetPlayer(string login)
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
            if (IsCurrentPlayer(newPlayer))
            {
                CurrentPlayer = newPlayer;
            }
        }

        public AbstractPlayer RegisterPlayer(string login, Position position)
        {
            var newPlayer = new Player(login, this, position);
            players.Add(login, newPlayer);
            return newPlayer;
        }
        
        public AbstractPlayer AddPlayerAtEmpty(string login)
        {
            var emptyPositions = new List<Position>();
            for (var i = 0; i < Board.Height; i++)
            {
                for (var j = 0; j < Board.Width; j++)
                {
                    var position = new Position(i, j);
                    if (Board.IsEmpty(position))
                    {
                        emptyPositions.Add(position);
                    }
                }
            }

            if (emptyPositions.Count == 0)
            {
                return null;
            }

            var random = new Random();
            var emptyPosition = emptyPositions[random.Next(emptyPositions.Count)];
            var newPlayer = RegisterPlayer(login, emptyPosition);
            Board.SetObject(emptyPosition, newPlayer);
            return newPlayer;
        }

        public void DeletePlayer(AbstractPlayer player)
        {
            players.Remove(player.Login);
        }

        public bool ContainsCharacter(string login)
        {
            return players.ContainsKey(login);
        }
    }
}
