using System.Collections.Generic;

namespace Roguelike.Model.PlayerModel
{
    public class PlayerManager
    {
        private readonly Dictionary<string, AbstractPlayer> players = new Dictionary<string, AbstractPlayer>();
        public AbstractPlayer CurrentPlayer { get; set; }

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

        public void RegisterPlayer(AbstractPlayer player)
        {
            players.Add(player.Login, player);
        }

        public void DeletePlayer(AbstractPlayer player)
        {
            players.Remove(player.Login);
        }

        public bool ContainsPlayer(string login)
        {
            return players.ContainsKey(login);
        }

        public AbstractPlayer GetPlayer(string login)
        {
            return players[login];
        }
    }
}