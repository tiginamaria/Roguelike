﻿using System;
using System.Collections.Generic;
using Roguelike.Model.Inventory;

namespace Roguelike.Model.PlayerModel
{
    public class AbstractPlayerFactory
    {
        public AbstractPlayer Create(string login, string type, Level level, Position position, 
            CharacterStatistics statistics, List<InventoryItem> inventory,  List<InventoryItem> appliedInventory)
        {
            return type switch
            {
                PlayerType.Player => new Player(login, level, position, statistics, inventory, appliedInventory),
                PlayerType.ConfusedPlayer => new ConfusedPlayer(level,
                    new Player(login, level, position, statistics, inventory, appliedInventory)),
                _ => throw new NotSupportedException()
            };
        }
    }
}