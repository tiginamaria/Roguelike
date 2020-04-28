using System;
using Roguelike.Model.Inventory;

namespace Roguelike.Model.PlayerModel
{
    public abstract class AbstractPlayer : Character
    {
        public EventHandler OnDie;

        public abstract void PutOff(string inventoryType);

        public abstract void PutOn(string inventoryType);
        
        protected AbstractPlayer(Position startPosition) : base(startPosition)
        {
        }
    }
}
