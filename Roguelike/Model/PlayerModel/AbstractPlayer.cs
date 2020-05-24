using System;
using System.Collections.Generic;
using Roguelike.Model.Inventory;

namespace Roguelike.Model.PlayerModel
{
    /// <summary>
    /// A basic class for player.
    /// </summary>
    public abstract class AbstractPlayer : Character
    {
        /// <summary>
        /// A unique player id.
        /// </summary>
        public string Login { get; }
        
        /// <summary>
        /// An event that is risen when the charter dies.
        /// </summary>
        public EventHandler OnDie;
        
        protected AbstractPlayer(Position startPosition, string login) : base(startPosition)
        {
            Login = login;
        }

        /// <summary>
        /// Deactivates the given inventory item.
        /// </summary>
        public abstract void PutOff(string inventoryType);
        
        /// <summary>
        /// Activates the given inventory item.
        /// </summary>
        public abstract void PutOn(string inventoryType);
        
        /// <summary>
        /// Returns player's inventory.
        /// </summary>
        public abstract List<InventoryItem> GetInventory();
        
        /// <summary>
        /// Returns active inventory items.
        /// </summary>
        public abstract List<InventoryItem> GetAppliedInventory();
    }
}
