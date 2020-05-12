using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly PlayerManager playerManager = new PlayerManager();

        public List<Mob> Mobs { get; }
        public Board Board { get; }
        public BoardGraph Graph { get; }

        public AbstractPlayer CurrentPlayer
        {
            get => playerManager.CurrentPlayer;
            set => playerManager.CurrentPlayer = value;
        }

        public Level(Func<Level, Board> boardCreator)
        {
            Board = boardCreator(this);
            Mobs = Board.FindMobs();
            Graph = new BoardGraph(Board);
            var playersList = Board.FindPlayers();
            foreach (var player in playersList)
            {
                playerManager.RegisterPlayer(player);
            }
        }

        public LevelSnapshot Save() => 
            new LevelSnapshot(new Board(Board.Width, Board.Height, Board.CloneGameObjects()));

        public AbstractPlayer GetPlayer(string login) => playerManager.GetPlayer(login);

        public Mob GetMob(string id) => Mobs.Find(mob => mob.Id == int.Parse(id));

        public bool IsCurrentPlayer(Character character) => playerManager.IsCurrentPlayer(character);

        public void UpdatePlayer(AbstractPlayer newPlayer) => playerManager.UpdatePlayer(newPlayer);

        public AbstractPlayer RegisterPlayer(string login, Position position)
        {
            var newPlayer = new Player(login, this, position);
            playerManager.RegisterPlayer(newPlayer);
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

        public void DeletePlayer(AbstractPlayer player) => playerManager.DeletePlayer(player);

        public bool ContainsPlayer(string login) => playerManager.ContainsPlayer(login);

        public Position? NearestPlayerPosition(Position position) => playerManager.Players.Values
            .OrderBy(p => Graph.GetDistance(position, p.Position))
            .FirstOrDefault()?.Position;
    }
}