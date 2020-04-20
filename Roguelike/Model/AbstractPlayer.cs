using System;

namespace Roguelike.Model
{
    public abstract class AbstractPlayer : Character
    {
        public EventHandler OnDie;
        
        protected AbstractPlayer(Position startPosition) : base(startPosition)
        {
        }
    }
}