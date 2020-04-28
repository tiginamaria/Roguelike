using System;
using System.Collections.Generic;
using System.Linq;
using Roguelike.Model.Inventory;

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
    
    public class AbstractPlayerFactory
    {
        public AbstractPlayer Create(string type, Level level, Position position, CharacterStatistics statistics, List<InventoryItem> inventory) {
            switch (type) {
                case PlayerType.Player:
                    return new Player(level, position, statistics, inventory);
                case PlayerType.ConfusedPlayer:
                    return new ConfusedPlayer(level, new Player(level, position, statistics, inventory));
                default:
                    throw new NotSupportedException();
            }
        }
    }
}