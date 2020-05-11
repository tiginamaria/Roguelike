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
        public AbstractPlayer Create(string login, string type, Level level, Position position, CharacterStatistics statistics, 
            List<InventoryItem> inventory,  List<InventoryItem> appliedInventory) {
            switch (type) {
                case PlayerType.Player:
                    return new Player(login, level, position, statistics, inventory, appliedInventory);
                case PlayerType.ConfusedPlayer:
                    return new ConfusedPlayer(level, new Player(login, level, position, statistics, inventory, appliedInventory));
                default:
                    throw new NotSupportedException();
            }
        }
    }
}