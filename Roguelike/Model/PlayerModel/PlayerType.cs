using System.Linq;

namespace Roguelike.Model.PlayerModel
{
    public class PlayerType
    {
        public const string Player = "$";
        public const string ConfusedPlayer = "?";
        
        public static bool Contains(string type)
        {
            return new[] {Player, ConfusedPlayer}.Contains(type);
        }
    }
}