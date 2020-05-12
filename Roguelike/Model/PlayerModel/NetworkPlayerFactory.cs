using System;
using System.Collections.Generic;
using Roguelike.Interaction;
using Roguelike.Model.Inventory;

namespace Roguelike.Model.PlayerModel
{
    public class NetworkPlayerFactory : PlayerFactory
    {
        private readonly ExitGameInteractor exitGameInteractor;

        public NetworkPlayerFactory(ExitGameInteractor exitGameInteractor) => 
            this.exitGameInteractor = exitGameInteractor;

        public override AbstractPlayer Create(string login, string type, Level level, Position position, 
            CharacterStatistics statistics, List<InventoryItem> inventory,  List<InventoryItem> appliedInventory)
        {
            var player = CreatePlayer(login, type, level, position, statistics, inventory, appliedInventory);
            player.OnDie += (sender, args) =>
            {
                exitGameInteractor.Exit(player);
            };
            return player;
        }

        private static AbstractPlayer CreatePlayer(string login, string type, Level level, Position position,
            CharacterStatistics statistics, List<InventoryItem> inventory, List<InventoryItem> appliedInventory)
        {
            return type switch
            {
                PlayerType.Player => new Player(login, level, position, statistics, inventory, appliedInventory),
                PlayerType.ConfusedPlayer => new ConfusedPlayer(level,
                    new Player(login, level, position, statistics, inventory, appliedInventory)),
                PlayerType.EnemyPlayer => new Player(login, level, position, statistics, inventory, appliedInventory),
                PlayerType.EnemyConfusedPlayer => new ConfusedPlayer(level,
                    new Player(login, level, position, statistics, inventory, appliedInventory)),
                _ => throw new NotSupportedException()
            };
        }
    }
}