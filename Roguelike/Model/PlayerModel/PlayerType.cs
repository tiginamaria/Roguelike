using System.Linq;

namespace Roguelike.Model.PlayerModel
{
    /// <summary>
    /// Returns a string representation of the player state.
    /// </summary>
    public static class PlayerType
    {
        public const string Player = "$";
        public const string ConfusedPlayer = "?";
        public const string EnemyPlayer = "J";
        public const string EnemyConfusedPlayer = "Q";
        
        public static bool Contains(string type) => 
            new[] {Player, ConfusedPlayer, EnemyPlayer, EnemyConfusedPlayer}.Contains(type);
    }
}