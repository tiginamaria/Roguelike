using System.Linq;

namespace Roguelike.Model.PlayerModel
{
    public class PlayerType
    {
        public const string Player = "$";
        public const string ConfusedPlayer = "?";
        public const string EnemyPlayer = "J";
        public const string EnemyConfusedPlayer = "Q";
        
        public static bool Contains(string type)
        {
            return new[] {Player, ConfusedPlayer, EnemyPlayer, EnemyConfusedPlayer}.Contains(type);
        }
    }
}