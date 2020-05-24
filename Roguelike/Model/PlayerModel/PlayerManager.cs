using System.Collections.Generic;

namespace Roguelike.Model.PlayerModel
{
    /// <summary>
    /// Tracks the players on board.
    /// Level.cs is used as its facade.
    /// </summary>
    public class PlayerManager
    {
        public AbstractPlayer CurrentPlayer { get; set; }

        public Dictionary<string, AbstractPlayer> Players { get; } = 
            new Dictionary<string, AbstractPlayer>();

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
            Players[newPlayer.Login] = newPlayer;
            if (IsCurrentPlayer(newPlayer))
            {
                CurrentPlayer = newPlayer;
            }
        }

        public void RegisterPlayer(AbstractPlayer player) => Players.Add(player.Login, player);

        public void DeletePlayer(AbstractPlayer player) => Players.Remove(player.Login);

        public bool ContainsPlayer(string login) => Players.ContainsKey(login);

        public AbstractPlayer GetPlayer(string login) => Players[login];
    }
}