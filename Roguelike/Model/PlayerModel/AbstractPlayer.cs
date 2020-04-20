using System;

namespace Roguelike.Model.PlayerModel
{
    public abstract class AbstractPlayer : Character
    {
        public EventHandler OnDie;
        
        protected AbstractPlayer(Position startPosition) : base(startPosition)
        {
        }
    }
}
