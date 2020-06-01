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
        private static readonly string OfflinePlayerLogin = "OfflinePlayer"; 
        private readonly PlayerManager playerManager = new PlayerManager();

        /// <summary>
        /// List of mobs on board.
        /// </summary>
        public List<Mob> Mobs { get; }
        
        /// <summary>
        /// A game board.
        /// </summary>
        public Board Board { get; }
        
        /// <summary>
        /// A board in a graph representation.
        /// </summary>
        public BoardGraph Graph { get; }

        /// <summary>
        /// A player controlled by the user.
        /// </summary>
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

        /// <summary>
        /// Returns a snapshot of the current game state.
        /// </summary>
        public LevelSnapshot Save() => 
            new LevelSnapshot(new Board(Board.Width, Board.Height, Board.CloneGameObjects()));

        /// <summary>
        /// Returns a player with the given login.
        /// </summary>
        public AbstractPlayer GetPlayer(string login) => playerManager.GetPlayer(login);

        /// <summary>
        /// Returns a mob with the given id.
        /// </summary>
        public Mob GetMob(string id) => Mobs.Find(mob => mob.Id == int.Parse(id));

        /// <summary>
        /// Checks whether the given character is controlled by user.
        /// </summary>
        public bool IsCurrentPlayer(Character character) => playerManager.IsCurrentPlayer(character);

        /// <summary>
        /// Substitutes the player with the new one saving its login.
        /// </summary>
        public void UpdatePlayer(AbstractPlayer newPlayer) => playerManager.UpdatePlayer(newPlayer);

        /// <summary>
        /// Adds a new player to the given position.
        /// </summary>
        public AbstractPlayer RegisterPlayer(string login, Position position)
        {
            var newPlayer = new Player(login, this, position);
            playerManager.RegisterPlayer(newPlayer);
            return newPlayer;
        }

        public AbstractPlayer AddOfflinePlayer(Position position)
        {
            var newPlayer = RegisterPlayer(OfflinePlayerLogin, position);
            Board.SetObject(position, newPlayer);
            CurrentPlayer = newPlayer;
            return newPlayer;
        }

        /// <summary>
        /// Adds player on a random empty position.
        /// Returns null if no empty positions found.
        /// </summary>
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

        /// <summary>
        /// Removes the given player from the game.
        /// </summary>
        public void DeletePlayer(AbstractPlayer player) => playerManager.DeletePlayer(player);

        /// <summary>
        /// Checks whether a player with the given login exists on board.
        /// </summary>
        public bool ContainsPlayer(string login) => playerManager.ContainsPlayer(login);

        /// <summary>
        /// Returns the nearest player to the given position.
        /// </summary>
        public Position? NearestPlayerPosition(Position position) => playerManager.Players.Values
            .OrderBy(p => Graph.GetDistance(position, p.Position))
            .FirstOrDefault()?.Position;
    }
}