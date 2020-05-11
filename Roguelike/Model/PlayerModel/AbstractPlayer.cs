using System;
using System.Collections.Generic;
using Roguelike.Model.Inventory;

namespace Roguelike.Model.PlayerModel
{
    public abstract class AbstractPlayer : Character
    {
        public string Login { get; }
        public EventHandler OnDie;
        
        protected AbstractPlayer(Position startPosition, string login) : base(startPosition)
        {
            Login = login;
        }

        public abstract void PutOff(string inventoryType);
        public abstract void PutOn(string inventoryType);
        public abstract List<InventoryItem> GetInventory();
        public abstract List<InventoryItem> GetAppliedInventory();
    }
}
